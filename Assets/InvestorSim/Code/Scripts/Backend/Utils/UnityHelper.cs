using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Представляет из себя класс с наиболее часто применяемыми методами в скриптах Unity
/// </summary>
internal class UnityHelper : MonoBehaviour
{
    private static List<string> dontDestroyNames = new List<string>()
    {
        "Soft Scrolling"
    };



    /// <summary>
    /// Удаляет все дочерние объекты указанного объекта
    /// </summary>
    /// <param name="parent">Объект родитель</param>
    internal static void DestroyAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (!dontDestroyNames.Contains(child.name))
            {
                Destroy(child.gameObject);
            }
        }
    }

    internal static void ResetRectTransform(GameObject obj)
    {
        var rectTransform = obj.GetComponent<RectTransform>();

        //rectTransform.offsetMin = new Vector2(0, 0);
        //rectTransform.offsetMax = new Vector2(1, 1);

        //rectTransform.

        // Устанавливаем положение и размеры
        rectTransform.anchoredPosition = Vector2.zero; // устанавливаем положение в центре экрана
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height); // устанавливаем размеры равные размеру экрана

        // Устанавливаем якорную точку
        rectTransform.anchorMin = new Vector2(0, 0); // устанавливаем нижний левый угол как якорную точку
        rectTransform.anchorMax = new Vector2(1, 1); // устанавливаем верхний правый угол как якорную точку

        // Устанавливаем отступы от краев родительского элемента
        rectTransform.offsetMin = Vector2.zero; // устанавливаем отступы слева и снизу в 0
        rectTransform.offsetMax = Vector2.zero; // устанавливаем отступы справа и сверху в 0
    }

    internal static void CrispText(Text text)
    {
        text.fontSize *= 10;
        text.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}