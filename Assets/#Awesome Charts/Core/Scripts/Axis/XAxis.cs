﻿using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    [RequireComponent(typeof(CanvasRenderer))]
    public class XAxis : AxisBase {

        [SerializeField]
        private float lineStep = 0;

        public float LineStep {
            get { return lineStep; }
            set { lineStep = value; }
        }
    }
}