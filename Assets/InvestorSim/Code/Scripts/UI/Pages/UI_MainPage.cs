using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Представляет из себя Главный раздел игры
/// </summary>
public class UI_MainPage : MonoBehaviour
{
    private void Awake()
    {

    }

    private void OnEnable()
    {
        Debug.Log(string.Format("Open main page\nname object: {0}", this.gameObject.name));
    }

    private void OnDisable()
    {
        Debug.Log(string.Format("Close main page\nname object: {0}", this.gameObject.name));
    }
}
