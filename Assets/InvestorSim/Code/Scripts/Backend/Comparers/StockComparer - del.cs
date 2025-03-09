using System.Collections.Generic;

/// <summary>
/// Сравнивает объекты класса Stock по их валюте, выводит хэш тикета
/// </summary>
public class StockComparer : IEqualityComparer<Stock>
{
    public bool Equals(Stock x, Stock y)
    {
        if (object.ReferenceEquals(x, y)) return true;

        if(object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return false;

        return x.Currency == y.Currency;
    }

    public int GetHashCode(Stock stock)
    {
        if (object.ReferenceEquals(stock, null)) return 0;

        int hashStockTicket = stock.Ticket == null ? 0 : stock.Ticket.GetHashCode();

        return hashStockTicket;
    }
}