using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]

/// <summary>
/// Контроллирует Animator всплывающего окна
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
    /// Открытие окна с анимацией Appear
    /// </summary>
    public virtual void OpenPopup()
    {
        this.gameObject.SetActive(true);
        animator.ResetTrigger("popup_Hide");
        animator.SetTrigger("popup_Appear");
    }

    /// <summary>
    /// Закрытие окна с анимацией Hide (Закрытие через событие в самом аниматоре
    /// </summary>
    public virtual void ClosePopup()
    {
        animator.ResetTrigger("popup_Appear");
        animator.SetTrigger("popup_Hide");
    }


    /// <summary>
    /// Отключение окна.
    /// </summary>
    private void DisablePopup()
    {
        this.gameObject.SetActive(false);
    }
}
