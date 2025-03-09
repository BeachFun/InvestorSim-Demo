using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]

/// <summary>
/// ��������� �������� ������� �� ������
/// </summary>
public class UI_Button : MonoBehaviour, /*IPointerClickHandler,*/ IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float scaleAnimationDuration = 0.3f;
    [SerializeField] private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 1f);
    [SerializeField] private Vector3 releasedScale = new Vector3(1f, 1f, 1f);

    private bool isPointerDown = false;
    private Coroutine scaleCoroutine;

    private void OnEnable()
    {
        transform.localScale = releasedScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPointerDown)
        {
            isPointerDown = true;
            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleTo(pressedScale));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPointerDown)
        {
            isPointerDown = false;
            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);
            scaleCoroutine = StartCoroutine(ScaleTo(releasedScale));
        }
    }

    public virtual void OnClick()
    {

    }

    /// <summary>
    /// �������������� �������� ������� gameObject � ��������� Scale
    /// </summary>
    /// <param name="targetScale">Scale ���������� � Vector3</param>
    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsedTime < scaleAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / scaleAnimationDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
