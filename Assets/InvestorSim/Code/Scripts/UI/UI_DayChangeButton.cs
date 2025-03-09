using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Контролирует анимацию нажатия на кнопку хода.
/// </summary>
public class UI_DayChangeButton : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Время анимации при нажатии")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Color targetColor = Color.yellow;


    private Image _background;
    private Color _initColor;
    private Coroutine _currCoroutine;


    private void Awake()
    {
        _background = GetComponent<Image>();
        _initColor = _background.color;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        AnimateButtonClick();
    }


    /// <summary>
    /// Запуск сопрограммы для анимации цвета
    /// </summary>
    public void AnimateButtonClick()
    {
        // Если уже есть запущенная анимация, то останавливаем ее
        if (_currCoroutine != null)
        {
            StopCoroutine(_currCoroutine);
        }

        // Запускаем новую анимацию
        //_currCoroutine = StartCoroutine(AnimateButtonColor());
    }

    private IEnumerator AnimateButtonColor()
    {
        float elapsedTime = 0f;

        // Изменяем цвет кнопки с помощью линейной интерполяции до целевого
        while (elapsedTime < animationDuration * 0.5f)
        {
            float t = elapsedTime / (animationDuration * 0.5f);
            _background.color = Color.Lerp(_initColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        elapsedTime = 0f;

        // Изменяем цвет кнопки с помощью линейной интерполяции до изначального
        while (elapsedTime < animationDuration * 0.5f)
        {
            float t = elapsedTime / (animationDuration * 0.5f);
            _background.color = Color.Lerp(targetColor, _initColor, t);

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _background.color = _initColor;
    }
}
