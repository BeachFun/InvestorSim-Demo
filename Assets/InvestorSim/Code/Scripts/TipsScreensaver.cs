using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// ���������� ����� � ���������
/// </summary>
public class TipsScreensaver : MonoBehaviour, IPointerDownHandler
{
    [Header("��������� ������ ��������")]
    [Tooltip("��������� ���� � �������� �� ����")]
    [SerializeField] private TMP_Text textTips;
    [Tooltip("������ ���")]
    [SerializeField] private Image currImage;
    [Tooltip("������ ��� ��� ������� ����� ��������")]
    [SerializeField] private Image nextImage;
    [Tooltip("������ ���")]
    [SerializeField] private GameObject background;
    [Space]
    [Tooltip("��������� ������ ����� ��� ����")]
    [SerializeField] private Sprite[] images;

    [Header("��������� �������")]
    [Tooltip("����� ������ �������� � ��������")]
    [SerializeField] private float imageSeconds = 14f;
    [Tooltip("����� ����� ��������")]
    [SerializeField] [Range(.1f, 2f)] private float crossfadeTime = 1.1f;
    [Tooltip("����� ������ ������ � ��������")]
    [SerializeField] private float tipSeconds = 3.3f;
    [Tooltip("����� �������� �� ��������")]
    [SerializeField][Range(.1f, 1)] private float blackoutDuration = .6f;
    [Tooltip("����� ����� ���������� ������������� ���")]
    [SerializeField][Range(.1f, 1)] private float textChangeDuration = .6f;


    private CanvasGroup _canvas;
    private Coroutine _tipCoroutine;
    private Coroutine _imageCoroutine;
    private int _curImageIndex;
    private float _c = 0.65f;


    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _curImageIndex = new System.Random().Next(images.Length);

        StartCoroutine(Blackout());

        Debug.Log("Screensaver opened");
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        StopCoroutine(_tipCoroutine);
        StopCoroutine(_imageCoroutine);

        this.gameObject.SetActive(false);
        currImage.sprite = null;

        Debug.Log("Screensaver closed");
    }


    /// <summary>
    /// ��������� �������� ����������� � ���������� ��������� �����
    /// </summary>
    /// <returns></returns>
    private IEnumerator Blackout()
    {
        // �������� �����-����� � ������� �������� ������������
        float elapsedTime = 0f;
        while (elapsedTime < blackoutDuration)
        {
            float t = elapsedTime / blackoutDuration;
            _canvas.alpha = t;

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _canvas.alpha = 1;

        _tipCoroutine = StartCoroutine(TipsController());
        _imageCoroutine = StartCoroutine(ImagesController());
    }

    /// <summary>
    /// ����������� �������� ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator TipsController()
    {
        yield return null;

        while (true)
        {
            if (textTips is not null)
            {
                float elapsedTime = 0f;
                while (elapsedTime < textChangeDuration * 0.5f)
                {
                    float t = elapsedTime / (textChangeDuration * 0.5f);
                    textTips.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(.4f, -1, t));

                    elapsedTime += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                textTips.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -1);

                textTips.text = GameTemplates.GetGameTips().GetRandom();

                elapsedTime = 0f;
                while (elapsedTime < textChangeDuration * 0.5f)
                {
                    float t = elapsedTime / (textChangeDuration * 0.5f);
                    textTips.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-1, .4f, t));

                    elapsedTime += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                textTips.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, .4f);
            }

            yield return new WaitForSeconds(tipSeconds);
        }
    }


    /// <summary>
    /// ����������� �������� ��������
    /// </summary>
    /// <returns></returns>
    private IEnumerator ImagesController()
    {
        yield return null;

        while (true)
        {
            if (currImage is not null)
            {
                StartCoroutine(ImageCrossfade());

                _curImageIndex++;
                if (_curImageIndex >= images.Length)
                    _curImageIndex = 0;
            }

            yield return new WaitForSeconds(imageSeconds);
        }
    }

    /// <summary>
    /// ������� ����� (������������) �������� �� ����� � ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator ImageCrossfade()
    {
        nextImage.color = new Color(_c, _c, _c, 0f);
        nextImage.sprite = images[_curImageIndex];
        nextImage.gameObject.SetActive(true);

        float time = 0;
        while (time < crossfadeTime)
        {
            float factor = time / crossfadeTime;

            currImage.color = new Color(_c, _c, _c, 1f - factor);
            nextImage.color = new Color(_c, _c, _c, factor);

            time += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        currImage.sprite = nextImage.sprite;
        currImage.color = nextImage.color;

        nextImage.gameObject.SetActive(false);
    }
}
