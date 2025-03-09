using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PreciousMetals : IAsset
{
    public decimal Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Currency { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Ticket { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public decimal FairPrice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    long IAsset.Amount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    public void ChangeFairPrice(decimal delta)
    {
        throw new NotImplementedException();
    }

    public void ChangePrice(decimal delta)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void PayOut()
    {
        throw new NotImplementedException();
    }
}