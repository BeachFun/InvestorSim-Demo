using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]

/// <summary>
/// ������������� Animator ������������ ����
/// </summary>
public class UI_Popup : MonoBehaviour, IPopup
{
    [SerializeField] private Animator animator;


    protected virtual void Start()
    {
        if (animator is null)
        {
            animator = GetComponent<Animator>();
        }
    }

    /// <summary>
    /// �������� ���� � ��������� Appear
    /// </summary>
    public virtual void OpenPopup()
    {
        this.gameObject.SetActive(true);
        animator.ResetTrigger("popup_Hide");
        animator.SetTrigger("popup_Appear");
    }

    /// <summary>
    /// �������� ���� � ��������� Hide (�������� ����� ������� � ����� ���������
    /// </summary>
    public virtual void ClosePopup()
    {
        animator.ResetTrigger("popup_Appear");
        animator.SetTrigger("popup_Hide");
    }


    /// <summary>
    /// ���������� ����.
    /// </summary>
    private void DisablePopup()
    {
        this.gameObject.SetActive(false);
    }
}
