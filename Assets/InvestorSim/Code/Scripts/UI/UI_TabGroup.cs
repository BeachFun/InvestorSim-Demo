using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������� �������������� ����� ��������� ��������� ����
/// </summary>
public class UI_TabGroup : MonoBehaviour
{
    // ! ����� ����� ��������� �������, ������� ������ ������������ ������� ������ ��������������� ������� �������

    [SerializeField] private List<UI_TabButton> tabButtons; // ������ ������������
    [SerializeField] private List<GameObject> sectionToSwap; // ������� ��� ������������

    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabActive;

    [SerializeField] private UI_TabButton prevSelectedTab;
    [SerializeField] private UI_TabButton selectedTab;


    /// <summary>
    /// ����� �� ����� ������� �������
    /// </summary>
    public int SelectedTubNumber
    {
        get
        {
            for(int i = 0; i < tabButtons.Count; i++)
                if (tabButtons[i] == selectedTab)
                    return i;

            return -1;
        } // TODO: ��������� ��� ��������� ������������ �������� �� ������� ��������
    }


    private void Start()
    {
        if (selectedTab.background != null)
        {
            selectedTab.AnimateButtonClick(tabIdle, tabActive);
        }
    }

    /// <summary>
    /// ����� ��� ��������� ������ ������������ ����� �������� ���������
    /// </summary>
    /// <param name="button">������</param>
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
    /// ������������ ������� �� ������
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
    /// ����������� �� ��������� ������� � �������
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
                Debug.Log("Selected Tab �� ���������� � UI Manager");
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
    /// ������� ���� ������ � ������� ���������
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
