using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Dialog_Comfirm : UI_Popup
{
    [Tooltip("Содержание окна подтверждения")]
    [SerializeField] private GameObject content;


    private Action _callback;


    private void OnEnable()
    {
        if (!Managers.Settings.Properties.ConfirmTransactions && _callback is not null)
        {
            _callback();
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _callback = null;
    }

    public void UpdateData(Action callback, GameObject content)
    {
        UnityHelper.DestroyAllChildren(this.content);
        content.transform.SetParent(this.content.transform);
        content.transform.localScale = Vector3.one;
        UnityHelper.ResetRectTransform(content);
        _callback = callback;
    }

    public void OnConfirm()
    {
        _callback();
        this.gameObject.SetActive(false);
    }
}
