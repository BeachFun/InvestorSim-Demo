using UnityEngine;
using TMPro;
using AwesomeCharts;

/// <summary>
/// ������������ �� ���� ����� ��� �������� ���������� �������������� ������ ������.
/// </summary>
public class UI_ShareCard : UI_Card
{
    [SerializeField] private TextMeshProUGUI textNameCompany;
    [SerializeField] private TextMeshProUGUI textTicket;
    [SerializeField] private TextMeshProUGUI textPriceInfo;
    [SerializeField] private TextMeshProUGUI textChangeRate;
    [SerializeField] private TextMeshProUGUI textCountInfo;
    [SerializeField] private UI_PriceGraph graph;

    /// <summary>
    /// �������� ��������, ����������� ������ ������
    /// </summary>
    public string NameCompany
    {
        get => textNameCompany.text;
        set
        {
            if (textNameCompany is not null) textNameCompany.text = value;
        }
    }
    /// <summary>
    /// ������������ ������ ������ (�����)
    /// </summary>
    public string Ticket
    {
        get => textTicket.text;
        set
        {
            if (textTicket is not null) textTicket.text = value;
        }
    }
    /// <summary>
    /// ���������� � ���� ������ ������
    /// </summary>
    public string PriceInfo
    {
        get => textPriceInfo.text;
        set
        {
            if (textPriceInfo is not null) textPriceInfo.text = value;
        }
    }
    /// <summary>
    /// ���������� � ��������� ����
    /// </summary>
    public decimal SetChangeRate
    {
        set
        {
            if (textChangeRate is not null)
            {
                if (value > 0)
                {
                    textChangeRate.text = string.Format("+{0:p2}", value);
                    textChangeRate.color = Color.green;
                }
                else if (value == 0)
                {
                    textChangeRate.text = string.Format("+{0:p2}", value);
                    textChangeRate.color = Color.gray;
                }
                else
                {
                    textChangeRate.text = string.Format("-{0:p2}", value);
                    textChangeRate.color = Color.red;
                }
            }
        }
    }
    /// <summary>
    /// ���������� ���-�� ������ ����� � ����
    /// </summary>
    public string CountInfo
    {
        get => textCountInfo.text;
        set
        {
            if (textCountInfo is not null) textCountInfo.text = value;
        }
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

        this.NameCompany = shares.CorporationName;
        this.Ticket = shares.Ticket;
        this.PriceInfo = string.Format("{0} {1}", SharpHelper.AddNumSpaces(shares.Price), shares.Currency);
        this.SetChangeRate = Controllers.Assets.GetDay1DeltaPrecent(shares);
        
        if (graph is not null)
        {
            graph.Data = Controllers.Assets.GetPriceStory(shares);
            graph.DrowGraph();
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


    public static implicit operator Shares(UI_ShareCard card)
    {
        // TODO: ���� ������ �������������� ��������� ��� �������
        return card.Shares;
    }
}
