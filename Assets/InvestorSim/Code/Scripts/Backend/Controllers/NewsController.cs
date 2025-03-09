using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Отвечает за хранение и генерацию новостей
/// </summary>
public class NewsController : MonoBehaviour, IGameController, IDataCopable
{
    public ControllerStatus status { get; private set; }

    /// <summary>
    /// История новостей
    /// </summary>
    public List<News> NewsStory { get => _model.NewsList.Where(x => !x.IsActually).ToList(); }
    /// <summary>
    /// Актуальные новости
    /// </summary>
    public List<News> ActuallyNews { get => _model.NewsList.Where(x => x.IsActually && x.ReleaseDate <= Managers.Game.Date).ToList(); }
    /// <summary>
    /// Не вышедшие новости
    /// </summary>
    public List<News> FutureNews { get => _model.NewsList.Where(x => x.ReleaseDate > Managers.Game.Date).ToList(); }


    private NewsModel _model;


    #region Методы запуска и Инициализации

    private void OnDestroy()
    {
        if (status == ControllerStatus.Started)
        {
            Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
            Messenger.RemoveListener(DataNotice.ASSETS_MODEL_UPDATED, AssetsModelUpdatedTrigger);
        }
    }

    public IEnumerator Startup()
    {
        status = ControllerStatus.Initializing;

        yield return null;

        // Если на момент запуска нет данных, то создаются данные по-умолчанию
        if (_model is null) _model = new NewsModel();

        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
        Messenger.AddListener(DataNotice.ASSETS_MODEL_UPDATED, AssetsModelUpdatedTrigger);

        status = ControllerStatus.Started;
    }

    public void UpdateData(NewsModel data)
    {
        _model = data;
    }

    #endregion


    /// <summary>
    /// Возвращает случайную NewsPower
    /// </summary>
    /// <returns>Объект перечисления NewsPower</returns>
    internal NewsPower RandomNewsPower()
    {
        return new System.Random().Next(0, 3) switch
        {
            0 => NewsPower.Low,
            1 => NewsPower.Medium,
            2 => NewsPower.High,
            _ => NewsPower.Unknown
        };
    }
    /// <summary>
    /// Возвращает случайную NewsPower по указанным коэффициентам
    /// </summary>
    /// <returns>Объект перечисления NewsPower</returns>
    internal NewsPower RandomNewsPower(byte low, byte medium, byte high)
    {
        int total = low + medium + high;
        int lowThreshold = low * 100 / total;
        int mediumThreshold = (low + medium) * 100 / total;

        int randomValue = new System.Random().Next(1, 101);

        if (randomValue <= lowThreshold)
            return NewsPower.Low;
        else if (randomValue <= mediumThreshold)
            return NewsPower.Medium;
        else
            return NewsPower.High;
    }


    /// <summary>
    /// Добавление новости в список новостей
    /// </summary>
    /// <param name="news">Новость</param>
    public void AddNews(News news)
    {
        _model.AddNews(news);
        Messenger.Broadcast(DataNotice.NEWS_MODEL_UPDATED);
    }

    /// <summary>
    /// Возвращает список актуальных новостей связанных с определенным активом
    /// </summary>
    /// <param name="asset">Актив</param>
    /// <returns>Список новостей</returns>
    public List<News> GetNewsList(IAsset asset)
    {
        return ActuallyNews.Where(x => x.Related.Keys.Contains(asset.Ticket)).ToList();
    }


    /// <summary>
    /// Возвращает предыдущую новость по списку актуальных новостей от указанной новости. Если указанной новости нет в спискох, вернет null
    /// </summary>
    /// <param name="news">Актуальная новость</param>
    /// <returns>Актуальная новость</returns>
    public News GetLastNews(News news)
    {
        int index = this.ActuallyNews.IndexOf(news);

        if (index == -1) return null;

        if (index - 1 < 0) return this.ActuallyNews[^1];

        return this.ActuallyNews[index - 1];
    }
    /// <summary>
    /// Возвращает следующую новость по списку актуальных новостей от указанной новости. Если указанной новости нет в спискох, вернет null
    /// </summary>
    /// <param name="news">Актуальная новость</param>
    /// <returns>Актуальная новость</returns>
    public News GetNextNews(News news)
    {
        int index = this.ActuallyNews.IndexOf(news);

        if (index == -1) return null;

        if (index + 1 > this.ActuallyNews.Count - 1) return this.ActuallyNews[0];

        return this.ActuallyNews[index + 1];
    }


    private void DayChangeTrigger()
    {
        // Debug.Log("News controller calc started");

        // News View Updating (MVC pattern)
        if (this.ActuallyNews.Any(x => x.ReleaseDate == Managers.Game.Date))
            Messenger.Broadcast(NewsNotice.NEWS_LIST_UPDATED);
    }

    private void AssetsModelUpdatedTrigger()
    {
        Messenger.Broadcast(DataNotice.NEWS_MODEL_UPDATED);
    }


    public object GetDataCopy()
    {
        return _model.Clone();
    }
}