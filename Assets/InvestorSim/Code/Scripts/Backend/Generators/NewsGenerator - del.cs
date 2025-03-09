using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Генерирует случайные или регулярные новости
/// </summary>
public class NewsGenerator
{
    public List<News> GenerateNews()
    {
        // TODO: Переименовать метода на что-то экзотическое

        List<News> list = new();
        list.AddRange(this.GetSharesFinStatementNews());

        return list;
    }

    /// <summary>
    /// Возвщает финансовые отчеты компании
    /// </summary>
    /// <returns>Список новостей о финансовых отчетах</returns>
    private List<News> GetSharesFinStatementNews()
    {
        Random rnd = new();
        List<News> allNews = new();

        List<Shares> allShares = Controllers.Assets.Data.Keys.Where(x => x is Shares).Select(x => x as Shares).ToList();
        foreach (Shares share in allShares)
        {
        }

        return allNews;
    }

    private List<News> GetSharesDividendsNews()
    {
        return null;
    }
}