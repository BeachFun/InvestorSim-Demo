using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Block_CurrencyExchange : MonoBehaviour
{
    [Header("Dropdown Zone")]
    [SerializeField] private TextMeshProUGUI textCurrency1Count;
    [SerializeField] private TextMeshProUGUI textCurrency2Count;
    [Tooltip("Валюта которую нужно обменять")]
    [SerializeField] private TMP_Dropdown dropdownCurrency1;
    [Tooltip("Валюта на которую нужно обменять")]
    [SerializeField] private TMP_Dropdown dropdownCurrency2;
    [SerializeField] private TMP_Text textCourse;
    [Header("Slider Zone")]
    [SerializeField] private Slider sliderCount;
    [SerializeField] private TMP_Text textSelectCount;
    [Header("Display Zone")]
    [SerializeField] private TMP_Text textCurrency1SwapCount;
    [SerializeField] private TMP_Text textCurrency2SwapCount;


    private List<string> _currencies1;
    private List<string> _currencies2;

    private string _currency1;
    private string _currency2;
    private decimal _sliderStep;

    private bool _initialized = false;


    private void Awake()
    {
        Messenger.AddListener(GameNotice.WALLET_CHANGE, OnEnable);
    }

    private void OnEnable()
    {
        if (!_initialized)
        {
            SetData();
            _initialized = true;
        }

        this.UpdatePage();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameNotice.WALLET_CHANGE, OnEnable);
    }


    private void SetData()
    {
        _currencies1 = Controllers.CurrencyExchange.Currencies.Keys.ToList();
        _currencies2 = Controllers.CurrencyExchange.GetSwapList(_currencies1[0]);

        dropdownCurrency1.ClearOptions();
        dropdownCurrency1.AddOptions(_currencies1.Select(x => x.ToString()).ToList());
        dropdownCurrency2.ClearOptions();
        dropdownCurrency2.AddOptions(_currencies2.Select(x => x.ToString()).ToList());

        dropdownCurrency1.onValueChanged.Invoke(0); // Нужен для первичного заполнения данными
    }

    private void UpdatePage()
    {
        // Настраивает Slider и остальные элементы для выбора кол-ва валют
        float minValue = (float)Controllers.CurrencyExchange.GetMinValueSwap(_currency1, _currency2);
        float maxValue = Convert.ToSingle(Managers.Player.Wallet[_currency1]);
        if (maxValue >= minValue)
        {
            sliderCount.minValue = minValue;
            sliderCount.maxValue = maxValue;
        }
        else
        {
            sliderCount.minValue = 0;
            sliderCount.maxValue = 0;
        }
        sliderCount.onValueChanged.Invoke(1);

        // Настройка отображания кол-ва выбранной валюты1 у игрока
        decimal currency1Value = Managers.Player.Wallet[_currency1];
        textCurrency1Count.text = string.Format("{0}", SharpHelper.AddNumSpaces(currency1Value));

        // Настройка отображания кол-ва выбранной валюты2 у игрока
        decimal currency2Value = Managers.Player.Wallet[_currency2];
        textCurrency2Count.text = string.Format("{0}", SharpHelper.AddNumSpaces(currency2Value));

        // Настройка отображения курса обмена
        decimal amount = Controllers.CurrencyExchange.GetCourse(_currency1, _currency2);
        textCourse.text = string.Format("1 {0} = {1} {2}", _currency2, amount.AddNumSpaces(), _currency1);
    }


    #region Обработчики для UI

    /// <summary>
    /// Обрабатывает выбор валюты в Dropdown1
    /// </summary>
    public void OnCurrency1Select()
    {
        _currency1 = _currencies1[dropdownCurrency1.value];
        _currencies2 = Controllers.CurrencyExchange.GetSwapList(_currency1);

        // Настройка второго выпадающего списка
        dropdownCurrency2.ClearOptions();
        dropdownCurrency2.AddOptions(_currencies2.Select(x => x.ToString()).ToList());
        dropdownCurrency2.onValueChanged.Invoke(0);
    }

    /// <summary>
    /// Обрабатывает выбор валюты в Dropdown1
    /// </summary>
    public void OnCurrency2Select()
    {
        _currency2 = _currencies2[dropdownCurrency2.value];

        this.UpdatePage();
    }

    public void OnSliderChange()
    {
        decimal currency1Count = (decimal)sliderCount.value;
        decimal currency2Count = Controllers.CurrencyExchange.CalcSwap(_currency1, currency1Count, _currency2);

        textSelectCount.text = string.Format("{0}", SharpHelper.AddNumSpaces(currency1Count));
        textCurrency1SwapCount.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(currency1Count), _currency1);
        textCurrency2SwapCount.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(currency2Count), _currency2);
    }

    public void Increment()
    {
        sliderCount.value += 1;
    }

    public void Decrement()
    {
        sliderCount.value -= 1;
    }

    public void Swap()
    {
        decimal currency1Count = (decimal)sliderCount.value;
        decimal currency2Count = Controllers.CurrencyExchange.CalcSwap(_currency1, currency1Count, _currency2);
        decimal comission = Controllers.CurrencyExchange.CalcComission(_currency1, currency1Count, _currency2);


        GameObject prefub = Instantiate(Resources.Load("UI/Block_Swap_Transaction_Info")) as GameObject;
        prefub.GetComponent<UI_SwapTransactionInfo>().UpdateData(_currency1, currency1Count, comission, _currency2, currency2Count);

        Action callback = new Action(() =>
        {
            Controllers.CurrencyExchange.Swap(_currency1, currency1Count, _currency2);
        });


        Managers.UI.OpenConfirmDialog(callback, prefub);
    }

    #endregion
}