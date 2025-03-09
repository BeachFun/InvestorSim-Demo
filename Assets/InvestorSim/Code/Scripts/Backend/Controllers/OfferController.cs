using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OfferController : MonoBehaviour, IGameController, IDataCopable
{
    public ControllerStatus status { get; private set; }

    /// <summary>
    /// Список предложений
    /// </summary>
    public List<Offer> OfferList { get => _model.OfferList; }
    /// <summary>
    /// Список новых предложений
    /// </summary>
    public List<Offer> NewOffers { get => _model.OfferList.Where(x => x.IsNew).ToList(); }
    /// <summary>
    /// Список актуальных предложений
    /// </summary>
    public List<Offer> ActuallyOffers { get => _model.OfferList.Where(x => x.Period > TimeSpan.Zero).ToList(); }
    /// <summary>
    /// Список с историей предложений
    /// </summary>
    public List<Offer> StoryList { get => _model.OfferList.Where(x => x.Period == TimeSpan.Zero).ToList(); }

    private OfferModel _model;


    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ControllerStatus.Started)
        {
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
            Messenger.RemoveListener(OfferControllerNotice.OFFER_NO_RELEVANT, UpdateList);
        }
    }

    public IEnumerator Startup()
    {
        status = ControllerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new OfferModel();

        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
        Messenger.AddListener(OfferControllerNotice.OFFER_NO_RELEVANT, UpdateList);

        status = ControllerStatus.Started;
        Debug.Log("Offer Controller started");
    }

    public void UpdateData(OfferModel data)
    {
        _model = data;
    }

    #endregion


    /// <summary>
    /// Добавление нового предложения вклада в список
    /// </summary>
    /// <param name="offer"></param>
    public void AddOffer(Offer offer)
    {
        _model.AddOffer(offer);
        Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);
    }

    /// <summary>
    /// Удаление определенного предложения вклада
    /// </summary>
    /// <returns>true - offer успешно удален</returns>
    public bool RemoveOffer(Offer offer)
    {
        bool res = _model.RemoveOffer(offer);

        if (res)
        {
            Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
            Messenger<Offer>.Broadcast(OfferControllerNotice.OFFER_REMOVED, offer);
            Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);
        }

        return res;
    }

    /// <summary>
    /// Обновление списка предложений. Отсеивает не актуальные предложения.
    /// </summary>
    public void UpdateList()
    {
        //_model.OfferList = OfferList.Where(x => x.Period.Days >= 0).ToList();
    }



    public bool AcceptOffer(decimal deposit, Offer offer)
    {
        // TODO: Подумать над удалением метода или его рефакторингом проекте

        if (!this.ActuallyOffers.Contains(offer))
            return false;

        IOfferTarget target = offer.OfferTarget;
        if (target is Investment)
        {
            var investment = (target as Investment).Clone() as Investment;

            investment.Start(deposit, Managers.Game.Date);
            Managers.Player.ChangeWalletMoney(offer.Currency, -deposit);
            Managers.Player.AddImvestment(investment);
            offer.Accept();
            Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
            Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);
            return true;
        }
        if (target is Dept)
        {
            var dept = (target as Dept).Clone() as Dept;

            if (dept.Lend())
            {
                Managers.Player.AddDept(dept);
                offer.Accept();
                Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
                Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);
                return true;
            }

            return false;
        }

        return false;
    }

    public bool CancelOffer(Offer offer)
    {
        try
        {
            offer.Accept();
        }
        catch
        {
            return false;
        }

        Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
        Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);

        return true;
    }

    /// <summary>
    /// Поиск предложения среди историии, актуальных и новых
    /// </summary>
    /// <param name="guid">Уникльный идентификатор</param>
    public Offer FindOffer(Guid guid)
    {
        return this.OfferList.Where(x => x.id == guid).First();
    }



    private void DayChangeTrigger()
    {
        System.Random rnd = new();

        // Действия в предложениях
        {
            List<Offer> offers = this.ActuallyOffers.ToList();
            foreach (Offer offer in offers)
            {
                offer.DayChangeTrigger();
            }
        }

        // Генерация предложений
        {
            // Предложения вклада в пирамиды. | Предложения появляются с определенной вероятностью, которая падает с каждой итерацией цикла
            for (int i = 2; i <= 5; i++)
            {
                if (rnd.Next(0, i * 2) == i * 2 - 1) // 25%, 16.5%, 12.5%
                {
                    Offer offer = GameTemplates.GetOffers1_FinancialPyramids().GetRandom();

                    this.AddOffer(offer);

                    Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
                    Messenger<Offer>.Broadcast(OfferControllerNotice.OFFER_NEW, offer);
                }
                else break;
            }

            // Шанс что попросят денег 10%
            if (rnd.Next(0, 10) == 0)
            {
                Offer offer = GameTemplates.GetOffers1_Deptors().GetRandom();

                this.AddOffer(offer);

                Messenger.Broadcast(OfferControllerNotice.OFFER_LIST_UPDATED);
                Messenger<Offer>.Broadcast(OfferControllerNotice.OFFER_NEW, offer);
            }
        }

        Messenger.Broadcast(DataNotice.OFFER_MODEL_UPDATED);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}