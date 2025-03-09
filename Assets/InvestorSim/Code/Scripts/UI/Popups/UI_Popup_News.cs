using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Popup_News : UI_Popup
{
    [SerializeField] private TMP_Text textTitle;
    [SerializeField] private TMP_Text textContent;
    [SerializeField] private TMP_Text textSource;
    [SerializeField] private TMP_Text textReleaseDate;
    [SerializeField] private TMP_Text textPasssedDays;
    [SerializeField] private TMP_Text textDurationDays;
    [SerializeField] private TMP_Text textNewsEssense;
    [SerializeField] private GameObject panelRelatedAssets;


    private News _news;


    protected override void Start()
    {
        base.Start();

        textContent.fontSize = textPasssedDays.fontSize;
        textDurationDays.fontSizeMax = textPasssedDays.fontSize;
    }

    public void UpdateData(News news)
    {
        _news = news;


        textTitle.text = news.Title;

        textContent.text = news.Content;

        textSource.text = news.Source;

        textReleaseDate.text = news.ReleaseDate.ToMyFormat();

        textPasssedDays.text = string.Format("День {0}", (Managers.Game.Date - news.ReleaseDate).Days);

        textDurationDays.text = string.Format("Длительность: {0} - {1}", news.DurationOfAction.Item1, news.DurationOfAction.Item2);

        // Настройка текстовой надписи "Что означает новость"
        textNewsEssense.color = news.Power switch
        {
            NewsPower.Low => Managers.UI.lowPower,
            NewsPower.Medium => Managers.UI.mediumPower,
            NewsPower.High => Managers.UI.highPower,
            _ => Managers.UI.lowPower
        };
        if (news.IsPositive)
        {
            textNewsEssense.text = "Рост";
        }
        else
        {
            textNewsEssense.text = news.Power switch
            {
                NewsPower.Low => "Падение",
                NewsPower.Medium => "Спад",
                NewsPower.High => "Обвал",
                _ => "Падение"
            };
        }

        UnityHelper.DestroyAllChildren(panelRelatedAssets);
        foreach (string assetTicket in news.Related.Keys)
        {
            IAsset asset = Controllers.Assets.FindAsset(assetTicket);
            if (asset is null) continue;
            CardBuilder.CreateAssetCard(panelRelatedAssets, asset);
        }
    }


    public override void OpenPopup()
    {
        base.OpenPopup();
        Messenger<UI_Popup_News>.Broadcast(UINotice.NEWS_POPUP_OPENED, this);
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        Messenger<UI_Popup_News>.Broadcast(UINotice.NEWS_POPUP_CLOSED, this);
    }


    public void ToPrevNews()
    {
        News news = Controllers.News.GetLastNews(_news);
        this.UpdateData(news);
    }

    public void ToNextNews()
    {
        News news = Controllers.News.GetNextNews(_news);
        this.UpdateData(news);
    }
}
