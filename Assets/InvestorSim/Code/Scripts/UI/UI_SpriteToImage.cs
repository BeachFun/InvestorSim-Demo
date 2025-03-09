using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Image))]

/// <summary>
/// Контроллер изменений в SpriteRenderer. Переносит менющееся картинку в Image
/// </summary>
public class UI_SpriteToImage : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 1f)] private float updateInterval = 0.01f;

    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private Coroutine _coroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();

        if (this.gameObject.activeSelf)
            _coroutine = StartCoroutine(UpdateSprite());
    }

    private void OnEnable()
    {
        if (_coroutine is not null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(UpdateSprite());
    }

    private void OnDisable()
    {
        if (_coroutine is not null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator UpdateSprite()
    {
        while (true)
        {
            try
            {
                _image.sprite = _spriteRenderer.sprite;
            }
            catch { }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
