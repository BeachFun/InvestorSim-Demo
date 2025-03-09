using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Header : MonoBehaviour
{
    [SerializeField] public TMP_Text caption;
    [Header("Text Settings")]
    [Tooltip("Цвет заднего фона")]
    [SerializeField] public Color backgroundColor;
    [Tooltip("Расположение текста по горизонтали")]
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
