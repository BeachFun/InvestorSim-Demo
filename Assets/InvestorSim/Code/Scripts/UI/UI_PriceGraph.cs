using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AwesomeCharts;

public class UI_PriceGraph : MonoBehaviour
{
    [SerializeField] private LineChart chart;
    [SerializeField] private uint dayCount = 7;
    [SerializeField] private uint lineCount = 7;
    [Header("��������� ������")]
    [Tooltip("���� �����")]
    [SerializeField] private Color lineColor = Color.white;
    [Tooltip("���� ���������� ��� ������")]
    [SerializeField] private Color fillColor = Color.black;


    /// <summary>
    /// ������ ������� (Item1 - ����, Item2 - ����/����)
    /// </summary>
    public List<(float, DateTime)> Data { get; set; }


    /// <summary>
    /// ����������� ���-�� ���� �� �������
    /// </summary>
    public uint DayCount
    {
        get => dayCount;
        set => dayCount = value;
    }


    /// <summary>
    /// ������/�������������� ������
    /// </summary>
    public void DrowGraph()
    {
        chart.GetChartData().DataSets.Clear();
        chart.GetChartData().CustomLabels.Clear();

        LineDataSet set = new LineDataSet();

        if (dayCount > Data.Count)
            for (int i = 0; i < Data.Count; i++) set.AddEntry(new LineEntry(i, Data[i].Item1));
        else 
            for (int i = 0; i < dayCount; i++) set.AddEntry(new LineEntry(i, Data[^((int)dayCount - i)].Item1));

        // TODO: �������� ����� ��� �� �������
        /* 
        int count = chart.XAxis.LinesCount;
        if (lineCount > Data.Count)
        {
            chart.XAxis.LinesCount = Data.Count;
            for(int i = 0; i < Data.Count; i++)
                chart.GetChartData().CustomLabels.Add(Data[i].Item2.ToString("d:MM"));
        }
        else
        {
            chart.XAxis.LinesCount = (int)lineCount;

            for (int i = 0; i < count; i++)
                chart.GetChartData().CustomLabels.Add(Data[^((int)dayCount - i)].Item2.ToString("d:MM"));
        }
        */

        set.LineColor = lineColor;
        set.FillColor = fillColor;

        chart.GetChartData().DataSets.Add(set);
        chart.SetDirty();
    }

    private void ConfigChart()
    {

    }


    public void FirstGraph() => dayCount = 7;

    public void SecondGraph() => dayCount = 35;

    public void ThirdGraph() => dayCount = 90;
}