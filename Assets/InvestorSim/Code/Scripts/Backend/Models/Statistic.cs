using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

public class Statistic : IGameModel
{
    public Statistic()
    {
        this.CapitalStory = new();
        TotalDeals = 0;
        SuccesfulDelas = 0;
        BestSuccesfulness = 0;
    }


    public Dictionary<int, decimal> CapitalStory { get; private set; }
    public int TotalDeals { get; private set; }
    public int SuccesfulDelas { get; private set; }
    public float BestSuccesfulness { get; private set; }


    public object Clone()
    {
        return new Statistic()
        {
            CapitalStory = this.CapitalStory.ToDictionary(pair => pair.Key, pair => pair.Value),
            TotalDeals = this.TotalDeals,
            SuccesfulDelas = this.SuccesfulDelas,
            BestSuccesfulness = this.BestSuccesfulness
        };
    }
}
