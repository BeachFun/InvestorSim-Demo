using UnityEngine;
using TMPro;

public class UI_SwapTransactionInfo : MonoBehaviour
{
    [Tooltip("Пара валют")]
    [SerializeField] private TextMeshProUGUI textSwapInfo;
    [Tooltip("Курс пары валют")]
    [SerializeField] private TextMeshProUGUI textCourseInfo;
    [Tooltip("Кол-во валюты для обмена")]
    [SerializeField] private TextMeshProUGUI textCurrency1Count;
    [Tooltip("Комиссия сделки")]
    [SerializeField] private TextMeshProUGUI textComission;
    [Tooltip("Общее кол-во полученной валюты")]
    [SerializeField] private TextMeshProUGUI textTotalSum;


    /// <summary>
    /// Обновление блока и установка информации об обмене валюты
    /// </summary>
    public void UpdateData(string currency1, decimal currency1Count, decimal comission, string currency2, decimal currency2Count)
    {
        decimal currency1Value = Controllers.CurrencyExchange.GetCourse(currency1, currency2);

        textSwapInfo.text = string.Format("{0} > {1}", currency1, currency2);

        textCourseInfo.text = string.Format("1 {0} = {1} {2}", currency2, currency1Value.AddNumSpaces(), currency1);

        textCurrency1Count.text = string.Format("{0} {1}", currency1Count.AddNumSpaces(), currency1);

        textComission.text = string.Format("{0} {1}", comission.AddNumSpaces(), currency2);

        textTotalSum.text = string.Format("{0} {1}", currency2Count.AddNumSpaces(), currency2);
    }
}