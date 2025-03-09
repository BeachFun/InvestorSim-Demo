using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class SimpleTooltip : MonoBehaviour, IPointerDownHandler
{
    public SimpleTooltipStyle simpleTooltipStyle;
    [TextArea] public string infoLeft = "Hello";
    [TextArea] public string infoRight = "";

    private STController tooltip;
    private bool isUIObject = false;


    private void Awake()
    {
        tooltip = Managers.UI.Tooltip;

        if (tooltip is null)
            Debug.LogWarning("На сцене нет объекта STController");

        if (GetComponent<RectTransform>())
            isUIObject = true;

        // Always make sure there's a style loaded
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isUIObject)
            return;
        ShowTooltip();
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    if (!isUIObject)
    //        return;
    //    HideTooltip();
    //}


    public void ShowTooltip()
    {
        tooltip.SetCustomStyledText(infoLeft, simpleTooltipStyle, STController.TextAlign.Left);
        tooltip.SetCustomStyledText(infoRight, simpleTooltipStyle, STController.TextAlign.Right);

        tooltip.ShowTooltip();
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }


    private void Reset()
    {
        // Load the default style if none is specified
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");

        // If UI, nothing else needs to be done
        if (GetComponent<RectTransform>())
            return;

        // If has a collider, nothing else needs to be done
        if (GetComponent<Collider>())
            return;

        // There were no colliders found when the component is added so we'll add a box collider by default
        // If you are making a 2D game you can change this to a BoxCollider2D for convenience
        // You can obviously still swap it manually in the editor but this should speed up development
        gameObject.AddComponent<BoxCollider>();
    }
}
