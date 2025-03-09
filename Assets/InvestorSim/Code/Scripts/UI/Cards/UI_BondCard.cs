using UnityEngine;
using TMPro;

public class UI_BondCard : UI_Card
{
    [SerializeField] private TextMeshProUGUI bondName; // Наименование
    [SerializeField] private TextMeshProUGUI term; // Срок
    [SerializeField] private TextMeshProUGUI priceInfo; // Цена
    [SerializeField] private TextMeshProUGUI annum; // Доходность

    /// <summary>
    /// Ссылка на акцию, которая представляет данная карточка
    /// </summary>
    public Bond Bonds { get; private set; }


    /// <summary>
    /// Обновляет данные карточки
    /// </summary>
    /// <param name="shares">Облигация</param>
    public void UpdateData(Bond bonds)
    {
        this.Bonds = bonds;

        bondName.text = string.Format("{0}", bonds.Name);

        term.text = string.Format("{0}", bonds.ExpirationSpan.ToReadableShortTimeSpan());

        priceInfo.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(bonds.Price), bonds.Currency);

        annum.text = string.Format("{0:p2}", bonds.CalcProfitPrecentToMaturity());
    }


    /// <summary>
    /// Обработчик на нажатие на карточку, отправляет данные всплывающему окну акции
    /// </summary>
    public override void OnClick()
    {
        base.OnClick();
        Managers.UI.OpenPopup(this);
    }
}
