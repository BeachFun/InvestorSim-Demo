using UnityEngine;
using TMPro;

public class UI_Assets_BondCard : UI_Card
{
    [SerializeField] private TextMeshProUGUI bondName; // Наименование
    [SerializeField] private TextMeshProUGUI remainedTerm; // Оставшийся срок
    [SerializeField] private TextMeshProUGUI cost; // Стоимость
    [SerializeField] private TextMeshProUGUI profitText; // Доход

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

        AssetPurchase asset = Managers.Player.FindAsset(AssetType.Bonds, bonds);

        if (asset is null)
            throw new UnityException("Такого актива нет у игрока!");

        bondName.text = string.Format("{0}", bonds.Name);

        remainedTerm.text = string.Format("осталось {0}", bonds.ExpirationSpan.ToReadableShortTimeSpan());

        cost.text = string.Format("{0} {1}", SharpHelper.AddNumSpaces(asset.Cost), bonds.Currency);

        decimal profitValue = asset.CalcProfit();
        if (profitValue > 0)
        {
            profitText.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profitValue), bonds.Currency, asset.CalcProfitPrecent());
            profitText.color = Color.green;
        }
        else if (profitValue == 0)
        {
            profitText.text = string.Format("+{0} {1} | +{2:p2}", SharpHelper.AddNumSpaces(profitValue), bonds.Currency, asset.CalcProfitPrecent());
            profitText.color = Color.gray;
        }
        else
        {
            profitText.text = string.Format("{0} {1} | {2:p2}", SharpHelper.AddNumSpaces(profitValue), bonds.Currency, asset.CalcProfitPrecent());
            profitText.color = Color.red;
        }
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