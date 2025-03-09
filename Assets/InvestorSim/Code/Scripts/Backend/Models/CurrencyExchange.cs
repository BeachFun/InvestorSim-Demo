using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]

public class CurrencyExchange : IGameModel
{
    public CurrencyExchange()
    {
        Currencies = new();
        Currencies.Add("$", 1);
        Currencies.Add("₽", 70);
        Currencies.Add("€", 0.9);
    }

    public Dictionary<string, double> Currencies { get; private set; }


    public object Clone()
    {
        return new CurrencyExchange()
        {
            Currencies = this.Currencies
        };
    }
}