using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]

public class UI_ToScreensaver : MonoBehaviour
{
    // TODO: класс не используюется в проекте

    [SerializeField] private string sceneName;
    [SerializeField] [Range(.1f, 1)] private float blackoutDuration = .6f;

    private CanvasGroup _canvas;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(Switch());
    }

    /// <summary>
    /// Загружает сцену заставки ассинхронно и постепенно затемняет экран
    /// </summary>
    /// <returns></returns>
    private IEnumerator Switch()
    {
        SceneManager.LoadSceneAsync(sceneName);

        // Изменяем альфа-канал с помощью линейной интерполяции
        float elapsedTime = 0f;
        while (elapsedTime < blackoutDuration)
        {
            float t = elapsedTime / blackoutDuration;
            _canvas.alpha = t;

            elapsedTime += .01f;
            yield return new WaitForSeconds(.01f);
        }

        _canvas.alpha = 1;
    }
}
