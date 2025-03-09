using UnityEngine;
using TMPro;

public class UI_BondCard : UI_Card
{
    [SerializeField] private TextMeshProUGUI bondName; // ������������
    [SerializeField] private TextMeshProUGUI term; // ����
    [SerializeField] private TextMeshProUGUI priceInfo; // ����
    [SerializeField] private TextMeshProUGUI annum; // ����������

    /// <summary>
    /// ������ �� �����, ������� ������������ ������ ��������
    /// </summary>
    public Bond Bonds { get; private set; }


    /// <summary>
    /// ��������� ������ ��������
    /// </summary>
    /// <param name="shares">���������</param>
    public void UpdateData(Bond bonds)
    {
        this.Bonds = bonds;

        bondName.text = string.Format("{0}", bonds.Name);

        term.text = string.Format("{0}", bonds.ExpirationSpan.ToReadableShortTimeSpan());

        priceInfo.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(bonds.Price), bonds.Currency);

        annum.text = string.Format("{0:p2}", bonds.CalcProfitPrecentToMaturity());
    }


    /// <summary>
    /// ���������� �� ������� �� ��������, ���������� ������ ������������ ���� �����
    /// </summary>
    public override void OnClick()
    {
        base.OnClick();
        Managers.UI.OpenPopup(this);
    }
}
