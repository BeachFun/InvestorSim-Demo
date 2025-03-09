using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Представление карточки новости
/// </summary>
public class UI_NewsCard : UI_Card
{
    [Header("Ссылки на компоненты")] [Space]
    [SerializeField] private Image imageNewsPower;
    [SerializeField] private Image imageNewsType;
    [SerializeField] private TMP_Text textDaysPassed;
    [SerializeField] private TMP_Text textNewsDuration;
    [SerializeField] private TMP_Text textCaption;
    [SerializeField] private TMP_Text textLink;
    [SerializeField] private TMP_Text textReleaseDate;

    private News _news;


    public void UpdateData(News news)
    {
        _news = news;

        Color color = news.Power switch
        {
            NewsPower.Low => Managers.UI.lowPower,
            NewsPower.Medium => Managers.UI.mediumPower,
            NewsPower.High => Managers.UI.highPower,
            _ => Color.white
        };

        KeyValuePair<string, Impulse> pair = news.Related.First(x => x.Value.IsActive);
        string ticket = pair.Key;
        string link = Controllers.Assets.GetName(ticket); // TODO: улучшить, сделать вывод сразу нескольких активов
        if (link is null) link = ticket; // TODO: улучшить, сделать вывод сразу нескольких активов


        if (textDaysPassed is not null) textDaysPassed.text = string.Format("{0}", pair.Value.Moment);
        else textDaysPassed.text = string.Empty;

        if (textNewsDuration is not null) textNewsDuration.text = string.Format("{0} - {1}", news.DurationOfAction.Item1, news.DurationOfAction.Item2);
        else textNewsDuration.text = string.Empty;

        if (textCaption is not null)
        {
            textCaption.color = color;
            textCaption.text = news.Title;
        }
        else textCaption.text = string.Empty;

        if (textLink is not null) textLink.text = link;
        else textLink.text = string.Empty;

        if (textReleaseDate is not null) textReleaseDate.text = news.ReleaseDate.ToString("d.MM.yyyy");
        else textReleaseDate.text = string.Empty;

        if (imageNewsPower is not null)
        {
            imageNewsPower.color = color;

            if (news.IsPositive) imageNewsPower.sprite = Resources.Load<Sprite>("Sprites/white-up-128px");
            else imageNewsPower.sprite = Resources.Load<Sprite>("Sprites/white-down-128px");
        }
    }

    public override void OnClick()
    {
        base.OnClick();
        Managers.UI.OpenNewsPopup(_news);
    }
}