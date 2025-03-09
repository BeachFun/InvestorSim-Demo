using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialog_Offer : UI_Popup
{
    [Tooltip("“екстовое поле с выводом введенного/выбранного депозита")]
    [SerializeField] private TextMeshProUGUI depositDisplay;
    [Header("Ёлементы ввода депозита")]
    [SerializeField] private GameObject panelInputDeposit;
    [SerializeField] private Slider sliderInput;
    [SerializeField] private TextMeshProUGUI inputDepositDisplay;
    [Header("Ёлементы выбора депозита")]
    [SerializeField] private GameObject panelChooseDeposit;
    [SerializeField] private TMP_Dropdown depositDropdown;


    private Offer _offer;
    private decimal _deposit;


    private void OnDisable()
    {
        panelChooseDeposit.SetActive(false);
        panelInputDeposit.SetActive(false);
    }

    public void UpdateData(Offer offer)
    {
        _offer = offer;

        if (offer.CanFlexibleDeposit)
        {
            panelInputDeposit.SetActive(true);

            decimal currencyCount = Managers.Player.Wallet[offer.Currency];
            if(offer.Max is not null)
            {
                sliderInput.maxValue = (float)(currencyCount < offer.Max ? currencyCount : offer.Max);
            }
            else
            {
                sliderInput.maxValue = (float)currencyCount;
            }

            sliderInput.minValue = (float)offer.Min;

            _deposit = offer.Min;
        }
        else
        {
            panelChooseDeposit.SetActive(true);

            List<string> listDep = offer.DepositList.Select(x => string.Format("{0} {1}", x, offer.Currency)).ToList();

            depositDropdown.ClearOptions();
            depositDropdown.AddOptions(listDep);

            _deposit = offer.DepositList.First();
        }

        depositDisplay.text = string.Format("{0} {1}", _deposit, offer.Currency);
    }

    /// <summary>
    /// »змен€ет текстовое отображение депозита при изменении выбора/ввода депозита
    /// </summary>
    public void OnDepositChanged()
    {
        if (_offer.CanFlexibleDeposit)
            // TODO: сделать изменение числа фиксированным равным offer.Step
            _deposit = (decimal)sliderInput.value;
        else
            _deposit = (decimal)_offer.DepositList[depositDropdown.value];

        depositDisplay.text = string.Format("{0} {1}", _deposit, _offer.Currency);
    }

    public void Increment() => sliderInput.value += (float)_offer.Step;

    public void Decrement() => sliderInput.value -= (float)_offer.Step;


    public void OnAccess()
    {
        Controllers.Offers.AcceptOffer(_deposit, _offer);
    }
}
