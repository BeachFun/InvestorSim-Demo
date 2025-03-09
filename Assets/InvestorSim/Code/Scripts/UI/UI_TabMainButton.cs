using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]

public class UI_TabMainButton : UI_TabButton
{
    [Header("Настройка контроллера перехода")]
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject mainPageMenu;
    [SerializeField] private GameObject stockExchSection;
    [SerializeField] private GameObject realEstateSection;
    [SerializeField] private GameObject dealershipSection;


    public override void OnClick()
    {
        if (mainPage.activeSelf)
        {
            Debug.Log("Переключение на главную с подвкладок");
            stockExchSection.SetActive(false);
            realEstateSection.SetActive(false);
            dealershipSection.SetActive(false);

            mainPageMenu.SetActive(true);
        }
        else
        {
            Debug.Log("Переключение на главную с другой страницы");
        }
    }
}
