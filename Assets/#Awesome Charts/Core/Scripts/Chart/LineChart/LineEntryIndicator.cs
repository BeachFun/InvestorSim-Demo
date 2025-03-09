using UnityEngine;
using UnityEngine.UI;

namespace AwesomeCharts {
    [RequireComponent(typeof(CanvasRenderer))]
    public class LineEntryIdicator : MonoBehaviour {

        public Button button;
        public Image image;
        public LineEntry entry;
    }
}
