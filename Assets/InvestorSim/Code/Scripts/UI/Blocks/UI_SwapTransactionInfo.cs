using UnityEngine;
using TMPro;

public class UI_SwapTransactionInfo : MonoBehaviour
{
    [Tooltip("���� �����")]
    [SerializeField] private TextMeshProUGUI textSwapInfo;
    [Tooltip("���� ���� �����")]
    [SerializeField] private TextMeshProUGUI textCourseInfo;
    [Tooltip("���-�� ������ ��� ������")]
    [SerializeField] private TextMeshProUGUI textCurrency1Count;
    [Tooltip("�������� ������")]
    [SerializeField] private TextMeshProUGUI textComission;
    [Tooltip("����� ���-�� ���������� ������")]
    [SerializeField] private TextMeshProUGUI textTotalSum;


    /// <summary>
    /// ���������� ����� � ��������� ���������� �� ������ ������
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