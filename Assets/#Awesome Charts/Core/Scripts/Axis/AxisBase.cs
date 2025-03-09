using UnityEngine;

namespace AwesomeCharts
{
    [System.Serializable]
    [RequireComponent(typeof(CanvasRenderer))]

    public class AxisBase
    {
        [SerializeField]
        private int lineThickness = Defaults.AXIS_LINE_THICKNESS;
        [SerializeField]
        private Color lineColor = Defaults.AXIS_LINE_COLOR;
        [SerializeField]
        private float minAxisValue = Defaults.AXIS_MIN_VALUE;
        [SerializeField]
        private float maxAxisValue = Defaults.AXIS_MAX_VALUE;
        [SerializeField]
        private bool autoAxisMinValue = false;
        [SerializeField]
        private bool autoAxisMaxValue = false;
        [SerializeField]
        private int linesCount = Defaults.AXIS_LINES_COUNT;
        [SerializeField]
        private int labelSize = Defaults.AXIS_LABEL_SIZE;
        [SerializeField]
        private Color labelColor = Defaults.AXIS_LABEL_COLOR;
        [SerializeField]
        private float labelMargin = Defaults.AXIS_LABEL_MARGIN;
        [SerializeField]
        private Font labelFont;
        [SerializeField]
        private FontStyle labelFontStyle;
        [SerializeField]
        private bool dashedLine = true;
        [SerializeField]
        private bool shouldDrawLabels = true;
        [SerializeField]
        private bool shouldDrawLines = true;

        public int LineThickness
        {
            get => lineThickness;
            set => lineThickness = value;
        }

        public Color LineColor
        {
            get => lineColor;
            set => lineColor = value;
        }

        public float MinAxisValue
        {
            get => minAxisValue;
            set => minAxisValue = value;
        }

        public float MaxAxisValue
        {
            get => maxAxisValue;
            set => maxAxisValue = value;
        }

        public bool AutoAxisMaxValue
        {
            get => autoAxisMaxValue;
            set => autoAxisMaxValue = value;
        }

        public bool AutoAxisMinValue
        {
            get => autoAxisMinValue;
            set => autoAxisMinValue = value;
        }

        public int LinesCount
        {
            get => linesCount;
            set => linesCount = value;
        }

        public int LabelSize
        {
            get => labelSize;
            set => labelSize = value;
        }

        public Color LabelColor
        {
            get => labelColor;
            set => labelColor = value;
        }

        public float LabelMargin
        {
            get => labelMargin;
            set => labelMargin = value;
        }

        public Font LabelFont
        {
            get => labelFont;
            set => labelFont = value;
        }

        public FontStyle LabelFontStyle
        {
            get => labelFontStyle;
            set => labelFontStyle = value;
        }

        public bool DashedLine
        {
            get => dashedLine;
            set => dashedLine = value;
        }

        public bool ShouldDrawLabels
        {
            get => shouldDrawLabels;
            set => shouldDrawLabels = value;
        }

        public bool ShouldDrawLines
        {
            get => shouldDrawLines;
            set => shouldDrawLines = value;
        }
    }
}