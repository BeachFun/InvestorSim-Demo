using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]

public class ProgressBar : MonoBehaviour
{
    [Header("Title Setting")]
    [SerializeField] private string title;
    [SerializeField] private Color titleColor;
    [SerializeField] private Font titleFont;
    [SerializeField] private int titleFontSize = 10;

    [Header("Bar Setting")]
    [SerializeField] private Color barColor;
    [SerializeField] private Color barBackGroundColor;
    [SerializeField] private Sprite barBackGroundSprite;
    [SerializeField] [Range(1f, 100f)] private int alert = 20;
    [SerializeField] private Color barAlertColor;

    [Header("Animation Setting")]
    [SerializeField] private float animationDuration = 0.5f;


    private Image bar;
    private Image barBackground;
    private Text txtTitle;
    private Coroutine curAnimation;


    public float BarValue { get; private set; }


    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        barBackground = transform.Find("BarBackground").GetComponent<Image>();
        txtTitle = transform.Find("Text").GetComponent<Text>();
    }

    private void Start()
    {
        txtTitle.text = title;
        txtTitle.color = titleColor;
        txtTitle.font = titleFont;
        txtTitle.fontSize = titleFontSize;

        bar.color = barColor;
        barBackground.color = barBackGroundColor;
        barBackground.sprite = barBackGroundSprite;

        UpdateView(BarValue);
    }


    public void UpdateValue(float value)
    {
        value = Mathf.Clamp(value, 0, 100);

        if (curAnimation is not null)
        {
            StopCoroutine(curAnimation);
            //Debug.Log("Stopped ProgressBar coroutine");
        }
        curAnimation = StartCoroutine(AnimateValue(bar.fillAmount * 100, value));
        BarValue = value;
    }

    private IEnumerator AnimateValue(float fromValue, float toValue)
    {
        //Debug.Log("Start ProgressBar coroutine");
        float time = 0f;
        float startValue = fromValue;
        while (time <= animationDuration)
        {
            time += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, toValue, time / animationDuration);

            UpdateView(currentValue);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        curAnimation = null;
    }

    private void UpdateView(float val)
    {
        bar.fillAmount = val / 100;
        txtTitle.text = string.Format("{0} {1:p0}", title, val / 100);

        if (alert >= val)
        {
            bar.color = barAlertColor;
        }
        else
        {
            bar.color = barColor;
        }
    }
}
