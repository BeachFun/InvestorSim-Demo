using UnityEngine;
using TMPro;

/// <summary>
/// Информация о покупке/продаже. Скрипт связывающий UI с данными.
/// </summary>
public class UI_TransactionInfo : MonoBehaviour
{
    [Tooltip("Заголовок окна")]
    [SerializeField] private TextMeshProUGUI caption;
    [Tooltip("Купленное кол-во")]
    [SerializeField] private TextMeshProUGUI count;
    [Tooltip("Сумма сделки")]
    [SerializeField] private TextMeshProUGUI sum;
    [Tooltip("Комиссия сделки")]
    [SerializeField] private TextMeshProUGUI comission;
    [Tooltip("Налог на прибыль от сделки")]
    [SerializeField] private TextMeshProUGUI tax;
    [SerializeField] private GameObject taxPanel;
    [SerializeField] private GameObject couponProfitPanel;
    [Tooltip("Накопленный купонный доход")]
    [SerializeField] private TextMeshProUGUI couponProfit;
    [Tooltip("Общая сумма")]
    [SerializeField] private TextMeshProUGUI totalSum;


    /// <summary>
    /// Обновление блока и установка информации об акции
    /// </summary>
    public void UpdateData(string caption, int count, decimal sum, decimal comission, decimal tax, decimal totalSum, string currency)
    {
        if (couponProfitPanel is not null)
            couponProfitPanel.gameObject.SetActive(false);

        if (tax <= 0)
            taxPanel.gameObject.SetActive(false);
        else
        {
            this.tax.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(tax), currency);
        }

        this.caption.text = caption;

        this.count.text = string.Format("{0} шт", count);

        this.sum.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(sum), currency);

        this.comission.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(comission), currency);

        this.totalSum.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(totalSum), currency);
    }

    /// <summary>
    /// Обновление блока и установка информации об облигации
    /// </summary>
    public void UpdateData(string caption, int count, decimal sum, decimal comission, decimal couponProfit, decimal tax, decimal totalSum, string currency)
    {
        UpdateData(caption, count, sum, comission, tax, totalSum, currency);

        if (this.couponProfitPanel is not null)
        {
            this.couponProfitPanel.gameObject.SetActive(true);
            this.couponProfit.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(couponProfit), currency);
        }
    }
}
