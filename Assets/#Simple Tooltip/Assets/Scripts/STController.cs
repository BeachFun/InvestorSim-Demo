using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class STController : MonoBehaviour
{
    [SerializeField] private GameObject background;

    public enum TextAlign { Left, Right };

    private Image panel;
    private TextMeshProUGUI toolTipTextLeft;
    private TextMeshProUGUI toolTipTextRight;
    private RectTransform rect;


    private void Awake()
    {
        // Load up both text layers
        var tmps = GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < tmps.Length; i++)
        {
            if (tmps[i].name == "_left")
                toolTipTextLeft = tmps[i];

            if (tmps[i].name == "_right")
                toolTipTextRight = tmps[i];
        }

        // Keep a reference for the panel image and transform
        panel = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        // Hide at the start
        HideTooltip();
    }


    private void ResizeToMatchText()
    {
        // Find the biggest height between both text layers
        var bounds = toolTipTextLeft.textBounds;
        float biggestY = toolTipTextLeft.textBounds.size.y;
        float rightY = toolTipTextRight.textBounds.size.y;
        if (rightY > biggestY)
            biggestY = rightY;

        // Dont forget to add the margins
        var margins = toolTipTextLeft.margin.y * 2;

        // Update the height of the tooltip panel
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, biggestY + margins);
    }

    public void ResizePanelToFitText(RectTransform panel, TextMeshProUGUI text1, TextMeshProUGUI text2)
    {
        // TODO: доработать метод расчета размера панели под 2 текста

        Bounds bounds1 = text1.textBounds;
        Bounds bounds2 = text2.textBounds;

        float y1 = bounds1.size.y;
        float x1 = bounds1.size.x;
        float y2 = bounds2.size.y;
        float x2 = bounds2.size.x;

        Debug.Log($"{y1} {y2} {x1} {x2}");

        Vector2 min = new Vector2(x1, y1);
        Vector2 max = new Vector2(x2, y2);

        //float text1Height = LayoutUtility.GetPreferredHeight(text1.rectTransform);
        //float text2Height = LayoutUtility.GetPreferredHeight(text2.rectTransform);
        //float panelHeight = text1Height + text2Height + 40;
        //panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
        panel.anchorMin = min;
        panel.anchorMax = max;
    }

    private void UpdateShow()
    {
        Vector3 vector = Input.mousePosition;
        rect.anchoredPosition = vector; // TODO: изменить на позицию от объекта
    }


    public void SetRawText(string text, TextAlign align = TextAlign.Left)
    {
        // Doesn't change style, just the text
        if(align == TextAlign.Left)
            toolTipTextLeft.text = text;
        if (align == TextAlign.Right)
            toolTipTextRight.text = text;
        ResizeToMatchText();
    }

    public void SetCustomStyledText(string text, SimpleTooltipStyle style, TextAlign align = TextAlign.Left)
    {
        if (text is null) text = "";

        // Update the panel sprite and color
        panel.sprite = style.slicedSprite;
        panel.color = style.color;

        // Update the font asset, size and default color
        toolTipTextLeft.font = style.fontAsset;
        toolTipTextLeft.color = style.defaultColor;
        toolTipTextRight.font = style.fontAsset;
        toolTipTextRight.color = style.defaultColor;

        // Convert all tags to TMPro markup
        var styles = style.fontStyles;
        for(int i = 0; i < styles.Length; i++)
        {
            string addTags = "</b></i></u></s>";
            addTags += "<color=#" + ColorToHex(styles[i].color) + ">";
            if (styles[i].bold) addTags += "<b>";
            if (styles[i].italic) addTags += "<i>";
            if (styles[i].underline) addTags += "<u>";
            if (styles[i].strikethrough) addTags += "<s>";
            text = text.Replace(styles[i].tag, addTags);
        }
        if (align == TextAlign.Left)
            toolTipTextLeft.text = text;
        if (align == TextAlign.Right)
            toolTipTextRight.text = text;
        ResizeToMatchText();
    }


    public string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
    }


    public void ShowTooltip()
    {
        this.gameObject.SetActive(true);
        this.background.SetActive(true);

        StartCoroutine(ShowTooltipAsync());
        //this.gameObject.SetActive(true);
        //this.background.SetActive(true);

        ////this.ResizePanelToFitText(rect, toolTipTextLeft, toolTipTextRight);
        //this.ResizeToMatchText();
        //this.UpdateShow();

        //StartCoroutine(this.AutoHide());
    }

    public void HideTooltip()
    {
        rect.anchoredPosition = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        this.gameObject.SetActive(false);
        this.background.SetActive(false);

        StopAllCoroutines();
    }


    private IEnumerator ShowTooltipAsync()
    {
        this.ResizeToMatchText();
        this.UpdateShow();

        yield return null;

        this.ResizeToMatchText();
        this.UpdateShow();

        yield return null;

        this.ResizeToMatchText();
        this.UpdateShow();

        yield return null;

        this.ResizeToMatchText();
        this.UpdateShow();

        StartCoroutine(this.AutoHide());
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(6.6f);

        if (this.gameObject.activeSelf)
            this.HideTooltip();
    }
}
