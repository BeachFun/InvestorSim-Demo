using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]

/// <summary>
/// ���������� �������� ��� ��������� ���
/// </summary>
public class DayChangeScreensaver : MonoBehaviour
{
    [SerializeField] private TMP_Text textCurrentDay;
    [Space]
    [Tooltip("����� �������� �� ��������")]
    [SerializeField][Range(.1f, 1)] private float blackoutDuration = .6f;
    [Tooltip("����� �������� � ����")]
    [SerializeField][Range(.1f, 1)] private float blackinDuration = .6f;
    [Tooltip("����� ����� ���������� ������������� ���")]
    [SerializeField][Range(.1f, 1)] private float textChangeDuration = .6f;


    private CanvasGroup _canvas;
    private Coroutine _blackoutCoroutine;
    private Coroutine _blackinCoroutine;
    private Coroutine _textCoroutine;
    private DateTime _currDay;
    private DateTime _nextDay;


    private void Awake()
    {
        this.gameObject.SetActive(false);
        _canvas = GetComponent<CanvasGroup>();

        Messenger.AddListener(GameNotice.DAY_CHANGE, Play);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameNotice.DAY_CHANGE, Play);
    }


    public void Play()
    {
        if (!Managers.Settings.Properties.DayChangeScreensaver)
            return;

        _currDay = Managers.Game.Date.AddDays(-1); // TODO: ����� ���� ��������, ���������� ������� ����� GregorianCalendar
        _nextDay = Managers.Game.Date;

        textCurrentDay.text = _currDay.ToString("d.MM.yyy");

        this.gameObject.SetActive(true);
        _blackoutCoroutine = StartCoroutine(Blackout());
    }

    public void Abort()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }


    /// <summary>
    /// ������������������ ���������� ������
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

        _textCoroutine = StartCoroutine(DaySwitch());
    }


    /// <summary>
    /// ����������������� ��������� ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator DaySwitch()
    {
        //TODO: ������������� �����, ��� ����� ����� �������� ���� ��������

        float elapsedTime = 0f;
        while (elapsedTime < textChangeDuration * 0.5f)
        {
            float t = elapsedTime / (textChangeDuration * 0.5f);
            textCurrentDay.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(0, -1, t));

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        textCurrentDay.text = _nextDay.ToString("d.MM.yyy");

        elapsedTime = 0f;
        while (elapsedTime < textChangeDuration * 0.5f)
        {
            float t = elapsedTime / (textChangeDuration * 0.5f);
            textCurrentDay.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-1, 0, t));

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _blackinCoroutine = StartCoroutine(Blackin());
    }

    /// <summary>
    /// ������������������ ���������� ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator Blackin()
    {
        _canvas.alpha = 1;

        // �������� �����-����� � ������� �������� ������������
        float elapsedTime = 0f;
        while (elapsedTime < blackinDuration)
        {
            float t = 1 - elapsedTime / blackinDuration;
            _canvas.alpha = t;

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _canvas.alpha = 0;

        this.gameObject.SetActive(false);
    }
}
