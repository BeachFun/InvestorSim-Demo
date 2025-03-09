using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UI_NewsPage : MonoBehaviour
{
    [SerializeField] private GameObject newsList;


    private void Awake()
    {
        UnityHelper.DestroyAllChildren(newsList); // TODO: ��������� �������
        CardBuilder.CreateHeader(newsList, "������ �������");
        CardBuilder.CreateHeader(newsList, "��������� �������");

        Messenger.AddListener(GameNotice.DAY_CHANGE, UpdatePage);
        Messenger.AddListener(NewsNotice.NEWS_LIST_UPDATED, UpdatePage);
    }

    private void OnEnable()
    {
        Debug.Log(string.Format("Open news page\nname object: {0}", this.gameObject.name));
        UpdatePage(); // TODO: �� ����������. ����� ����� ������ ���� �����
    }

    private void OnDisable()
    {
        Debug.Log(string.Format("Close news page\nname object: {0}", this.gameObject.name));
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(NewsNotice.NEWS_LIST_UPDATED, UpdatePage);
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, UpdatePage);
    }


    public void UpdatePage()
    {
        UnityHelper.DestroyAllChildren(newsList);

        List<News> news = Controllers.News.ActuallyNews;
        List<News> freshNews = news.Where(x => x.ReleaseDate == Managers.Game.Date).Reverse().ToList();
        List<News> otherNews = news.Where(x => x.ReleaseDate != Managers.Game.Date).Reverse().ToList();

        CardBuilder.CreateHeader(newsList, "������ �������");
        if (freshNews.Count > 0)
        {
            foreach (News item in freshNews)
                CardBuilder.CreateNewsCard(newsList, item);
        }
        else
        {
            CardBuilder.CreatePlugCard(newsList, "��� ������ ��������");
        }

        CardBuilder.CreateHeader(newsList, "��������� �������");
        if (otherNews.Count > 0)
        {
            foreach (News item in otherNews)
                CardBuilder.CreateNewsCard(newsList, item);
        }
        else
        {
            CardBuilder.CreatePlugCard(newsList, "�����");
        }
    }
}