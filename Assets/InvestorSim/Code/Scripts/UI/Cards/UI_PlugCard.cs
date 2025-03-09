using UnityEngine;
using TMPro;

/// <summary>
/// ������������ �� ���� ���������� �������� � ��������� ������ � ���� (������ ��������)
/// </summary>
public class UI_PlugCard: UI_Card
{
    // TODO: �������� ��������, �� �� ��� � ����� ������������ ���������� � �������������� ������� �� ����������

    [SerializeField] private TMP_Text caption;

    /// <summary>
    /// ������������� ��������� ����� ��������
    /// </summary>
    public string CaptionText
    {
        get => caption.text;
        set => caption.text = value;
    }

    /// <summary>
    /// ������ �� ��������� ����� ��� ����� ������ ���������
    /// </summary>
    public TMP_Text Caption
    {
        get => caption;
    }

    private void Awake()
    {
        CaptionText = "�����";
    }

    public void UpdateData(string caption)
    {
        this.CaptionText = caption;
    }
}