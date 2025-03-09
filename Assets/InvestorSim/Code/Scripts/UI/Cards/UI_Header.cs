using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Header : MonoBehaviour
{
    [SerializeField] public TMP_Text caption;
    [Header("Text Settings")]
    [Tooltip("���� ������� ����")]
    [SerializeField] public Color backgroundColor;
    [Tooltip("������������ ������ �� �����������")]
    [SerializeField] public HorizontalAlignmentOptions horizontalAlignment;
    [Header("Tooltip Settings")]
    [SerializeField] public SimpleTooltip tooltip;
    [SerializeField] public SimpleTooltipStyle tooltipStyle;
    [SerializeField] public bool showingTooltip;

    private void Start()
    {
        this.gameObject.GetComponent<Image>().color = backgroundColor;

        caption.horizontalAlignment = horizontalAlignment;
        tooltip.simpleTooltipStyle = tooltipStyle;

        if (showingTooltip)
        {
            tooltip.gameObject.SetActive(true);
        }
        else
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
