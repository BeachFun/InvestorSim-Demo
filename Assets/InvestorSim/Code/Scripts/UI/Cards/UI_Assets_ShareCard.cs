using UnityEngine;
using TMPro;

public class UI_Assets_ShareCard : UI_Card
{
    [SerializeField] private TextMeshProUGUI nameCompany;
    [SerializeField] private TextMeshProUGUI currentCost;
    [SerializeField] private TextMeshProUGUI countAndPrice;
    [SerializeField] private TextMeshProUGUI change;

    /// <summary>
    /// �������� ��������, ����������� ������ ������
    /// </summary>
    public string NameCompany
    {
        get => nameCompany.text;
        set => nameCompany.text = value;
    }
    /// <summary>
    /// ������� ��������� ������ �����
    /// </summary>
    public string CurrentCost
    {
        get => currentCost.text;
        set => currentCost.text = value;
    }
    /// <summary>
    /// ���������� � ���-�� � ������� �������� ��������� ������ �����
    /// </summary>
    public string CountAndPrice
    {
        get => countAndPrice.text;
        set => countAndPrice.text = value;
    }
    /// <summary>
    /// ���������� �� ��������� ��������� ������ �����
    /// </summary>
    public string Change
    {
        get => change.text;
        set => change.text = value;
    }


    /// <summary>
    /// ������ �� �����, ������� ������������ ������ ��������
    /// </summary>
    public Shares Shares { get; private set; }


    /// <summary>
    /// ��������� ������ ��������
    /// </summary>
    /// <param name="shares">�����</param>
    public void UpdateData(Shares shares)
    {
        this.Shares = shares;

        AssetPurchase asset = Managers.Player.FindAsset(AssetType.Shares, shares);

        if (asset is null)
            throw new UnityException("������ ������ ��� � ������!");

        NameCompany = string.Format("{0}", shares.CorporationName);

        CurrentCost = string.Format("{0} {1}", SharpHelper.AddNumSpaces(asset.Amount * shares.Price), shares.Currency);

        CountAndPrice = string.Format("{0} �� | {1} {2}", SharpHelper.AddNumSpaces(asset.Amount), SharpHelper.AddNumSpaces(shares.Price), shares.Currency);

        decimal profitValue = asset.CalcProfit();
        if (profitValue > 0)
        {
            change.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profitValue), shares.Currency, asset.CalcProfitPrecent());
            change.color = Color.green;
        }
        else if (profitValue == 0)
        {
            change.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profitValue), shares.Currency, asset.CalcProfitPrecent());
            change.color = Color.gray;
        }
        else
        {
            change.text = string.Format("{0} {1} | {2:p2}", SharpHelper.AddNumSpaces(profitValue), shares.Currency, asset.CalcProfitPrecent());
            change.color = Color.red;
        }
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
