using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiceRoll
{
    public class ScoreViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private TextMeshProUGUI totalText;
        [SerializeField] private Button rollButton;

        private void Awake()
        {
            resultText.text = "";
            totalText.text = "";
        }

        public Button.ButtonClickedEvent RollButtonClicked
        {
            get { return rollButton.onClick; }
            set { rollButton.onClick = value; }
        }

        public void UpdateScore(int score, int total)
        {
            totalText.text = total.ToString();
            resultText.text = score.ToString();
        }

        public void SetRollingText()
        {
            resultText.text = "?";
        }
    }
}