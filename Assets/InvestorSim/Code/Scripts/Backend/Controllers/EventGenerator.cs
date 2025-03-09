using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Генератор событий в игре. Является контроллером
/// </summary>
public class EventGenerator : MonoBehaviour, IGameController
{
    [Tooltip("Шанс судебных разбирательств с какой-либо компанией в игровом дне")]
    [Range(.1f, 100f)] public float litigationChance = .1f;
    [Tooltip("Шанс принятия стратегических решений какой-либо компанией в игровом дне")]
    [Range(.1f, 100f)] public float strategicВecisionChance = .1f;


    public ControllerStatus status { get; private set; }


    private void Awake()
    {
        //Messenger.AddListener<bool>(StartupNotice.PRIMARY_PRICES_GENERATION, DayChangeTrigger);
        Messenger.AddListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, DayChangeTrigger);
    }

    public IEnumerator Startup()
    {
        status = ControllerStatus.Started;
        Debug.Log("Offer Controller started");

        yield return null;
    }


    private void /*bool*/ DayChangeTrigger()
    {
        System.Random rnd = new();
        DateTime currentDate = Managers.Game.Date;

        try
        {
            this.SharesHandler(rnd, currentDate);
            this.BondHandler(rnd, currentDate);
        }
        catch
        {
            Debug.Log("Ошибка при генерации событий");
        }

        //return true;
    }

    private void SharesHandler(System.Random rnd, DateTime currentDate)
    {
        // Объявление о выплатах дивидендов
        List<Corporation> corpList = Controllers.Corporations.List.Where(x => new DateTime((currentDate - x.FoundationDate).Ticks).Month % 3 == 1 && currentDate.Day == x.FoundationDate.Day + 6).ToList();
        foreach (Corporation corporation in corpList)
        {
            if (Controllers.Corporations.dividendAnnounceChance > UnityEngine.Random.Range(0, 100))
            {
                float min = Controllers.Corporations.dinidendLowBorder;
                float max = Controllers.Corporations.dinidendHighBorder;
                decimal rate = (decimal)UnityEngine.Random.Range(min / 100, max / 100);

                NewsPower newsPower = NewsPower.Low;
                if (((float)(rate * 100) / (max - min) * 100) > 45)
                    newsPower = NewsPower.Medium;
                if (((float)(rate * 100) / (max - min) * 100) > 70)
                    newsPower = NewsPower.High;

                if (corporation.SharesOrdTicket is not null)
                {
                    Shares shares = Controllers.Assets.FindAsset(corporation.SharesOrdTicket) as Shares;
                    Dividend dividend = corporation.AnnounceDividend(ShareType.Ordinary, shares.Price * rate);
                    var impulse = new Impulse((decimal)((int)newsPower * 0.3), rnd.Next(2, 5), 0);
                    var assetsDict = new Dictionary<string, Impulse>();
                    assetsDict.TryAdd(corporation.SharesOrdTicket, impulse);

                    Controllers.News.AddNews(new News() // TODO: сделать взятие рандомного шаблона из GameTemplates
                    {
                        Title = "Выплата дивидендов",
                        Content = string.Format("Компания {0} объявила о выплате дивидендов своим акционерам в размере {1} {2} на одну акцию.",
                                    corporation.Name, dividend.DividendValue.AddNumSpaces(), shares.Currency),
                        Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                        IsPositive = true,
                        Power = newsPower,
                        Related = assetsDict,
                        ReleaseDate = currentDate,
                        DurationOfAction = (2, 5)
                    });

                }
                if (corporation.SharesPrivTicket is not null)
                {
                    Shares shares = Controllers.Assets.FindAsset(corporation.SharesPrivTicket) as Shares;
                    Dividend dividend = corporation.AnnounceDividend(ShareType.Privileged, shares.Price * rate);
                    var impulse = new Impulse((decimal)((int)newsPower * 0.3), rnd.Next(2, 5), 0);
                    var assetsDict = new Dictionary<string, Impulse>();
                    assetsDict.TryAdd(corporation.SharesPrivTicket, impulse);

                    Controllers.News.AddNews(new News() // TODO: сделать взятие рандомного шаблона из GameTemplates
                    {
                        Title = "Выплата дивидендов",
                        Content = string.Format("Компания {0} объявила о выплате дивидендов своим акционерам в размере {1} {2} на одну акцию.",
                                    corporation.Name, dividend.DividendValue.AddNumSpaces(), shares.Currency),
                        Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                        IsPositive = true,
                        Power = newsPower,
                        Related = assetsDict,
                        ReleaseDate = currentDate,
                        DurationOfAction = (2, 5)
                    });
                }
            }
            else
            {
                try
                {
                    Shares sharesOrd = Controllers.Assets.FindAsset(corporation.SharesOrdTicket) as Shares;
                    Shares sharesPriv = Controllers.Assets.FindAsset(corporation.SharesPrivTicket) as Shares;

                    NewsPower power = NewsPower.Medium;
                    var impulse = new Impulse(-(decimal)((int)power * 0.5), rnd.Next(3, 6), 0);
                    var assetsDict = new Dictionary<string, Impulse>();
                    if (corporation.SharesOrdTicket != null) assetsDict.TryAdd(corporation.SharesOrdTicket, impulse);
                    if (corporation.SharesPrivTicket != null) assetsDict.TryAdd(corporation.SharesPrivTicket, impulse);

                    Controllers.News.AddNews(new News() // TODO: сделать взятие рандомного шаблона из GameTemplates
                    {
                        Title = "Выплата дивидендов",
                        Content = $"Выплата дивидендов по акциям компании {corporation.Name} отменяется",
                        Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                        IsPositive = false,
                        Power = power,
                        Related = assetsDict,
                        ReleaseDate = currentDate,
                        DurationOfAction = (3, 6)
                    });

                    Messenger<Shares>.Broadcast(CorporationNotice.DIVIDEND_ANNOUNCED_CANCELED, sharesOrd);
                    Messenger<Shares>.Broadcast(CorporationNotice.DIVIDEND_ANNOUNCED_CANCELED, sharesPriv);
                }
                catch (Exception ex)
                {
                    Debug.Log("Ошибка генерации события - Отмена выплат дивидендов | " + ex.Message + ex.StackTrace);
                }
            }
        }

        // Объявление о финансовых результатов | 50 35 15
        corpList = Controllers.Corporations.List.Where(x => new DateTime((currentDate - x.FoundationDate).Ticks).Month % 3 == 0 && currentDate.Day == x.FoundationDate.Day).ToList();
        foreach (Corporation corporation in corpList)
        {
            try
            {
                NewsPower power = Controllers.News.RandomNewsPower(50, 35, 15); // TODO: Жесткая зависимость
                bool isPositive = Controllers.Corporations.financeStatementChance > rnd.Next(0, 100);
                var impulse = new Impulse(isPositive ? (int)((int)power * 0.7) : -(int)((int)power * 0.7), rnd.Next(2, 4), 0);
                var assetsDict = new Dictionary<string, Impulse>();
                if (corporation.SharesOrdTicket != null) assetsDict.TryAdd(corporation.SharesOrdTicket, impulse);
                if (corporation.SharesPrivTicket != null) assetsDict.TryAdd(corporation.SharesPrivTicket, impulse);

                News news = new News() // TODO: сделать взятие рандомного шаблона из GameTemplates
                {
                    Title = "Финансовый отчет",
                    Content = GameTemplates.GetFinResult(isPositive, power, corporation.Name).GetRandom(),
                    Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                    IsPositive = isPositive,
                    Power = power,
                    Related = assetsDict,
                    ReleaseDate = currentDate,
                    DurationOfAction = (2, 4)
                };

                Controllers.News.AddNews(news);
                Messenger<News>.Broadcast(CorporationNotice.FINANCE_STATEMENT_RELEASED, news);
            }
            catch (Exception ex)
            {
                Debug.Log("Ошибка генерации события - Объявление о финансовых результатов | " + ex.Message + ex.StackTrace);
            }
        }

        // Шанс возникновения события "Судебные разбирательства" со случайной компанией
        if (litigationChance > rnd.Next(100))
        {
            try
            {
                Corporation corporation = Controllers.Corporations.List.GetRandom();

                NewsPower power = SharpHelper.GetRandom(NewsPower.Medium, NewsPower.High);
                var impulse = new Impulse(-(int)power, rnd.Next(3, 7), 0);
                var assetsDict = new Dictionary<string, Impulse>();
                if (corporation.SharesOrdTicket != null) assetsDict.TryAdd(corporation.SharesOrdTicket, impulse);
                if (corporation.SharesPrivTicket != null) assetsDict.TryAdd(corporation.SharesPrivTicket, impulse);

                Controllers.News.AddNews(new News()
                {
                    Title = "Судебные разбирательства",
                    Content = $"Компания {corporation.Name} столкнулась с судебным разбирательством из-за предполагаемого нарушения закона.",
                    Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                    IsPositive = false,
                    Power = power,
                    Related = assetsDict,
                    ReleaseDate = currentDate,
                    DurationOfAction = (3, 7)
                });
                Messenger<Corporation>.Broadcast(CorporationNotice.FINANCE_STATEMENT_RELEASED, corporation);
            }
            catch (Exception ex)
            {
                Debug.Log("Ошибка генерации события - Судебные разбирательства | " + ex.Message + ex.StackTrace);
            }
        }

        // Шанс возникновения события "Стратегическое решение" со случайной компанией
        if (strategicВecisionChance > rnd.Next(100))
        {
            try
            {
                Corporation corporation = Controllers.Corporations.List.GetRandom();

                NewsPower power = SharpHelper.GetRandom(NewsPower.Medium, NewsPower.High);
                var impulse = new Impulse((int)power, rnd.Next(3, 7), 0);
                var assetsDict = new Dictionary<string, Impulse>();
                if (corporation.SharesOrdTicket != null) assetsDict.TryAdd(corporation.SharesOrdTicket, impulse);
                if (corporation.SharesPrivTicket != null) assetsDict.TryAdd(corporation.SharesPrivTicket, impulse);

                Controllers.News.AddNews(new News()
                {
                    Title = "Стратегическое решение",
                    Content = GameTemplates.GetStrategicBec(corporation.Name).GetRandom(),
                    Source = "Журнал \"Фондовая Газета\"", // TODO: Добавить различные полноценные самостоятельные новостные издания (компании, тг каналы)
                    IsPositive = true,
                    Power = power,
                    Related = assetsDict,
                    ReleaseDate = currentDate,
                    DurationOfAction = (3, 7)
                });
                Messenger<Corporation>.Broadcast(CorporationNotice.FINANCE_STATEMENT_RELEASED, corporation);
            }
            catch (Exception ex)
            {
                Debug.Log($"Ошибка генерации события - Судебные разбирательства | {ex.Message} {ex.StackTrace}");
            }
        }
    }

    private void BondHandler(System.Random rnd, DateTime currentDate)
    {

    }
}