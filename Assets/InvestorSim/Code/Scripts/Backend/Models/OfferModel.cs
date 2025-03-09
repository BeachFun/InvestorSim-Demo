using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]

public class OfferModel : IGameModel
{
    public OfferModel()
    {
        OfferList = new();
    }

    public List<Offer> OfferList { get; set; }

    public void AddOffer(Offer offer)
    {
        OfferList.Add(offer);
    }

    public bool RemoveOffer(Offer offer)
    {
        return OfferList.Remove(offer);
    }


    public object Clone()
    {
        List<Dictionary<string, object>> data = this.OfferList.Select(x => x.GetDataCopy()).ToList();
        return data;
    }
}
