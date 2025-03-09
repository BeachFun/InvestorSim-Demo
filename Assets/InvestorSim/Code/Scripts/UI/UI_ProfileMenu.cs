using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

/// <summary>
/// ���������� ��������� ���� �������
/// </summary>
public class UI_ProfileMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;

    protected virtual void Start()
    {
        if (animator is null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void OpenMenu()
    {
        this.gameObject.SetActive(true);
        animator.ResetTrigger("menu_Hide");
        animator.SetTrigger("menu_Appear");
    }

    public void CloseMenu()
    {
        animator.ResetTrigger("menu_Appear");
        animator.SetTrigger("menu_Hide");
    }

    /// <summary>
    /// ���������� ����
    /// </summary>
    private void DisablePopup()
    {
        this.gameObject.SetActive(false);
    }
}
