using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct CouponInfo
{
    /// <summary>
    /// Величина купона
    /// </summary>
    public decimal CouponValue
    {
        get;
        set;
    }
    /// <summary>
    /// Частота выплат
    /// </summary>
    public PaymentFrequency PaymentFrequency
    {
        get;
        set;
    }
}
