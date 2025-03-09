using UnityEngine;
using TMPro;

/// <summary>
/// Представляет из себя контроллер элемента с текстовой меткой в себе (пустая страница)
/// </summary>
public class UI_PlugCard: UI_Card
{
    // TODO: Добавить проверку, на то что к полям присоеденены компоненты и информирование системы об отсутствии

    [SerializeField] private TMP_Text caption;

    /// <summary>
    /// Устанавливает текстовую метку карточке
    /// </summary>
    public string CaptionText
    {
        get => caption.text;
        set => caption.text = value;
    }

    /// <summary>
    /// Ссылка на текстовую метку для более точной настройки
    /// </summary>
    public TMP_Text Caption
    {
        get => caption;
    }

    private void Awake()
    {
        CaptionText = "Пусто";
    }

    public void UpdateData(string caption)
    {
        this.CaptionText = caption;
    }
}