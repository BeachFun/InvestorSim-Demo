using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет переключениями между основными вкладками игры
/// </summary>
public class UI_TabGroup : MonoBehaviour
{
    // ! Чтобы класс корректно работал, порядок кнопки переключения вкладки должен соответствовать порядку вкладок

    [SerializeField] private List<UI_TabButton> tabButtons; // кнопки переключения
    [SerializeField] private List<GameObject> sectionToSwap; // разделы для переключения

    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabActive;

    [SerializeField] private UI_TabButton prevSelectedTab;
    [SerializeField] private UI_TabButton selectedTab;


    /// <summary>
    /// Какая по счету активна вкладка
    /// </summary>
    public int SelectedTubNumber
    {
        get
        {
            for(int i = 0; i < tabButtons.Count; i++)
                if (tabButtons[i] == selectedTab)
                    return i;

            return -1;
        } // TODO: докончить для механизма переключения разделов на главной странице
    }


    private void Start()
    {
        if (selectedTab.background != null)
        {
            selectedTab.AnimateButtonClick(tabIdle, tabActive);
        }
    }

    /// <summary>
    /// Нужен для занесения кнопок переключения между главными вкладками
    /// </summary>
    /// <param name="button">Кнопка</param>
    public void Subscribe(UI_TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<UI_TabButton>();
        }

        button.background.color = tabIdle;

        tabButtons.Add(button);
    }

    /// <summary>
    /// Обрабатывает нажатия на кнопку
    /// </summary>
    /// <param name="button"></param>
    public void OnTabSelected(UI_TabButton button)
    {
        selectedTab = button;
        ResetTabs();

        int index = button.transform.GetSiblingIndex();
        SwitchTab(index);
    }


    /// <summary>
    /// Переключает на указанную вкладку в порядке
    /// </summary>
    public void SwitchTab(int index)
    {
        prevSelectedTab = selectedTab;

        try
        {
            if (selectedTab.background != null)
            {
                selectedTab.AnimateButtonClick(tabIdle, tabActive);
            }
            else
            {
                Debug.Log("Selected Tab не прикреплен к UI Manager");
            }

            for (int i = 0; i < tabButtons.Count; i++)
            {
                if (i == index)
                {
                    sectionToSwap[i].SetActive(true);
                }
                else
                {
                    sectionToSwap[i].SetActive(false);
                }
            }
        }
        catch { }
    }


    /// <summary>
    /// Перевод всех кнопок в обычное состояние
    /// </summary>
    private void ResetTabs()
    {
        prevSelectedTab.AnimateButtonClick(tabActive, tabIdle);

        foreach (UI_TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }

            button.background.color = tabIdle;
        }
    }
}
