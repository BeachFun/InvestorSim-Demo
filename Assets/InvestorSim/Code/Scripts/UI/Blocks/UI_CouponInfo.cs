using System;
using UnityEngine;
using TMPro;

public class UI_CouponInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI couponSize;
    [SerializeField] private TextMeshProUGUI payDate;
    [SerializeField] private TextMeshProUGUI PaymentInYear;
    [SerializeField] private TextMeshProUGUI PayCountRemained;
    [SerializeField] private TextMeshProUGUI couponRate;

    private Coupon _coupon;

    public void UpdateData(Coupon coupon, string currency)
    {
        DateTime payDate = coupon.GetNextPayDate();

        if (this.couponSize is not null && payDate != DateTime.MinValue)
            this.couponSize.text = string.Format("{0:f2} / {1:f2} {2}", coupon.CalcCouponProfit(), coupon.CouponValue, currency);
        else
            this.couponSize.text = string.Format("{0:f2} {1}", coupon.CalcCouponProfit(), currency);

        if (this.payDate is not null && payDate != DateTime.MinValue)
            this.payDate.text = string.Format("{0}", payDate.ToMyFormat());
        else
            this.payDate.text = string.Empty;

        if (this.PaymentInYear is not null)
            this.PaymentInYear.text = string.Format("{0}", (int)coupon.PaymentFrequency / 10);

        if (this.PayCountRemained is not null)
            this.PayCountRemained.text = string.Format("{0}", coupon.GetPayDayRemained());

        if (this.couponRate is not null)
            this.couponRate.text = string.Format("{0:p2}", coupon.CouponRate);

        this._coupon = coupon;
    }
}
