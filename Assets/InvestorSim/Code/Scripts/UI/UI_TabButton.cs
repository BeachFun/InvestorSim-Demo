using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]

public class UI_TabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UI_TabGroup tabGroup;
    [SerializeField] private float animationDuration = 0.3f;

    public Image background;
    private Button _button;
    private Color _startColor;
    private Color _endColor;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        background = GetComponent<Image>();
        _button = GetComponent<Button>();

        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
        tabGroup.OnTabSelected(this);
    }

    /// <summary>
    /// Запуск корутинки для анимации цвета
    /// </summary>
    public void AnimateButtonClick(Color startColor, Color endColor)
    {
        // Если уже есть запущенная анимация, то останавливаем ее
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        _startColor = startColor;
        _endColor = endColor;

        // Запускаем новую анимацию
        currentCoroutine = StartCoroutine(AnimateButtonColor());
    }

    private IEnumerator AnimateButtonColor()
    {
        float elapsedTime = 0f;

        // Изменяем цвет кнопки с помощью линейной интерполяции
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            background.color = Color.Lerp(_startColor, _endColor, t);

            elapsedTime += .01f /*Time.deltaTime*/;
            yield return new WaitForSeconds(.01f/*Time.deltaTime*/);
        }

        background.color = _endColor;
    }

    public virtual void OnClick()
    {

    }
}
