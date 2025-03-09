using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]

public class NewsModel : IGameModel
{
    public NewsModel()
    {
        this.NewsList = new List<News>();
    }

    /// <summary>
    /// Список новостей | не вышедшие, свежие и прошлые новости
    /// </summary>
    public List<News> NewsList { get; private set; }


    public void AddNews(News news)
    {
        NewsList.Add(news);
        Messenger.Broadcast(NewsNotice.NEWS_LIST_UPDATED);
    }


    public object Clone()
    {
        return new NewsModel()
        {
            NewsList = this.NewsList.Select(x => x.Clone() as News).ToList()
        };
    }
}