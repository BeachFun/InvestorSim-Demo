using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Список валют с механизмом объединения объектов
/// </summary>
public class CurrencyFilter1 : MultivaluedFilter<CurrencyFilter1>
{
    public static readonly CurrencyFilter1 RUB = MultivaluedFilter<CurrencyFilter1>.RegisterPossibleValue(1uL, "₽");

    public static readonly CurrencyFilter1 USD = MultivaluedFilter<CurrencyFilter1>.RegisterPossibleValue(2uL, "$");

    public static readonly CurrencyFilter1 EUR = MultivaluedFilter<CurrencyFilter1>.RegisterPossibleValue(4uL, "€");

    public static CurrencyFilter1 GetCurrency(string ticket)
    {
        if (ticket == RUB.ToString()) return RUB;
        if (ticket == USD.ToString()) return USD;
        if (ticket == EUR.ToString()) return EUR;

        return null;
    }
}