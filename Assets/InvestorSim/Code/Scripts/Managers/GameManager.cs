using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Отвечает за управление временем в игре, обработку действий пользователя и реализацию логики игры.
/// </summary>
public class GameManager : MonoBehaviour, IGameManager, IDataCopable
{
    [Tooltip("День старта игры")]
    [SerializeField] private DateTime startGameDate = new DateTime(2020, 1, 1);

    /// <summary>
    /// Статус менеджера
    /// </summary>
    public ManagerStatus status { get; private set; }

    /// <summary>
    /// Текущий день игры
    /// </summary>
    public DateTime Date { get => _model.Date; }
    /// <summary>
    /// Кол-во игровых дней
    /// </summary>
    public int DaysInGame { get => _model.DaysInGame; }


    internal bool GameStarted { get; private set; } // не сохранять


    private GameModel _model;


    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ManagerStatus.Started)
        {
            Messenger.RemoveListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);
        }
    }

    public IEnumerator Startup()
    {
        Messenger.Broadcast(StartupNotice.GAME_MANAGER_STARTING);
        status = ManagerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new GameModel(startGameDate);

        Messenger.AddListener(StartupNotice.ALL_CONTROLLERS_STARTED, this.AllControllersStartedTrigger);

        status = ManagerStatus.Started;
        Messenger.Broadcast(StartupNotice.GAME_MANAGER_STARTED);
    }

    public void UpdateData(GameModel model)
    {
        _model = model;
    }

    #endregion


    public void NextDay()
    {
        _model.AddDay();
        Messenger.Broadcast(DataNotice.GAME_MODEL_UPDATED);

        //if (_model.Date.DayOfWeek == DayOfWeek.Monday)
        //{
        //    Managers.Player.ChangeWalletMoney(CurrencyFilter.RUB, 5000);
        //}

        //if (_model.Date.Day == 1)
        //{
        //    Managers.Player.ChangeWalletMoney(Controllers.CurrencyExchange.RUB, 10000);
        //    // TODO: сделать оповещение о выплате
        //}
    }

    /// <summary>
    /// Перезапуск игры, сброс игрового результата
    /// </summary>
    public void Restart()
    {
        Messenger.Broadcast(GameNotice.RESTART);;
        SceneManager.LoadSceneAsync(0);
    }


    private void AllControllersStartedTrigger()
    {
        GameStarted = true;
        Debug.Log("Игра запущена");
        Messenger.Broadcast(StartupNotice.GAME_STARTED);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}
