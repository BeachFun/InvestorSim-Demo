using System;

//namespace InvestorSim.Model
//{

/// <summary>
/// Модель компании содержащей сведения о состоянии компании
/// </summary>
public class Company : Organization, IAsset
{
#pragma warning disable CS0414 // Полю "Company._type" присвоено значение, но оно ни разу не использовано.
    private CompanyType _type = CompanyType.LLC; // 
#pragma warning restore CS0414 // Полю "Company._type" присвоено значение, но оно ни разу не использовано.

    public decimal Price { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string Currency { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public string Ticket { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public long Amount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public decimal FairPrice { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    public void ChangeFairPrice(decimal delta)
    {
        throw new System.NotImplementedException();
    }

    public void ChangePrice(decimal delta)
    {
        if (Price + delta < 0)
        {
            Price = 0;
        }
        else
        {
            Price += delta;
        }
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void PayOut()
    {
        throw new System.NotImplementedException();
    }
}
//}