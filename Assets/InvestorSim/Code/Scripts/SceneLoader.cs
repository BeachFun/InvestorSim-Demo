using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [Header("Настройки экрана загрузки")]
    [Tooltip("Текстовое поле с советами по игре")]
    [SerializeField] private TMP_Text textTips;
    [Tooltip("Задний фон")]
    [SerializeField] private Image ImageBackground;
    [Tooltip("Коллекция задних фонов для игры")]
    [SerializeField] private Sprite[] images;
    [SerializeField] private ProgressBar progressBar;
    [Tooltip("Время показа экрана загрузки в секундах")]
    [SerializeField] private float loadTimeSeconds = 11f;
    [Tooltip("Время показа совета в секундах")]
    [SerializeField] private float tipsSeconds = 3.3f;


    private void Start()
    {
        StartCoroutine(LoadScene());
        StartCoroutine(Controller());
    }


    /// <summary>
    /// Загрузка главной сцены с отложенной загрузкой в 11 с.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene()
    {
        if (images.Length != 0)
            ImageBackground.sprite = images.ToList().GetRandom();

        progressBar.UpdateValue(1);
        yield return new WaitForSeconds(loadTimeSeconds/40);

        progressBar.UpdateValue(33);
        yield return new WaitForSeconds(loadTimeSeconds/16);

        progressBar.UpdateValue(51);
        yield return new WaitForSeconds(loadTimeSeconds/32);

        progressBar.UpdateValue(73);
        yield return new WaitForSeconds(loadTimeSeconds/11);

        progressBar.UpdateValue(99);
        yield return new WaitForSeconds(loadTimeSeconds/1.94f);

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// Сопрограмма меняющая советы
    /// </summary>
    /// <returns></returns>
    private IEnumerator Controller()
    {
        for (int i = 0; i < 7; i++)
        {
            if (textTips is not null)
            {
                textTips.text = GameTemplates.GetGameTips().GetRandom();
            }

            yield return new WaitForSeconds(tipsSeconds);
        }
    }
}
