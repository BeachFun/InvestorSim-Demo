using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using AwesomeCharts;

public class UI_Section_Statistic : MonoBehaviour
{
    [SerializeField] private LineChart chart;
    [SerializeField] private TMP_Text textDay;
    [SerializeField] private TMP_Text textOwnFounds;
    [SerializeField] private TMP_Text textCurrenciesValue;
    [SerializeField] private TMP_Text textStocksValue;
    [SerializeField] private TMP_Text textTotalDeals;
    [SerializeField] private TMP_Text textSuccesfullDeals;
    [SerializeField] private TMP_Text textSuccesfulness;
    [SerializeField] private TMP_Text textBestSuccesfulness; // TODO: подумать над вынесением в отдельный файл
    [Header("Настройка графика")]
    [SerializeField] private Color fillColor;


    private void OnEnable()
    {
        UpdateSection();
    }


    public void UpdateSection()
    {
        DrowGraph();

        string currecy = Managers.Settings.Properties.MainCurrency;

        textDay.text = Managers.Game.DaysInGame.ToString();
        textOwnFounds.text = string.Format("{0} {1}", Controllers.Statistic.OwnFunds.ShortenNumberString(), currecy);
        textCurrenciesValue.text = string.Format("{0} {1}", Controllers.Statistic.CurrenciesValue.ShortenNumberString(), currecy);
        textStocksValue.text = string.Format("{0} {1}", Controllers.Statistic.StocksValue.ShortenNumberString(), currecy);
        textTotalDeals.text = Controllers.Statistic.TotalDeals.ToString();
        textSuccesfullDeals.text = Controllers.Statistic.SuccesfulDelas.ToString();
        textBestSuccesfulness.text = string.Format("{0:p2}", Controllers.Statistic.BestSuccesfulness.ToString());
        textSuccesfulness.text = string.Format("{0:p2}", Controllers.Statistic.Succesfulness);
    }

    /// <summary>
    /// Рисует/Перерисовывает график
    /// </summary>
    private void DrowGraph()
    {
        Dictionary<int, decimal> data = Controllers.Statistic.CapitalStory;
        data = data.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);

        chart.GetChartData().DataSets.Clear();

        LineDataSet set = new LineDataSet();
        foreach (int day in data.Keys)
            set.AddEntry(new LineEntry(day, (float)data[day]));

        set.FillColor = fillColor;

        chart.GetChartData().DataSets.Add(set);
        chart.SetDirty();

    }
}
