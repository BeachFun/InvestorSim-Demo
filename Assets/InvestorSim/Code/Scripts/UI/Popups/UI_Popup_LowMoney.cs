using System.Collections;
using UnityEngine;
using TMPro;

public class UI_Popup_LowMoney : UI_Popup
{
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] [Range(.5f, 5f)] private float noticeDuration = 1.1f;


    private Coroutine _currCoroutine;


    private void OnEnable()
    {
        OpenPopup();
        _currCoroutine = StartCoroutine(Close());
    }


    public override void ClosePopup()
    {
        if (_currCoroutine is not null)
            StopCoroutine(_currCoroutine);

        base.ClosePopup();
        this.gameObject.SetActive(true);
    }


    private IEnumerator Close()
    {
        yield return new WaitForSeconds(noticeDuration);

        this.ClosePopup();
    }
}
