using UnityEngine;
using UnityEngine.Events;

namespace DiceRoll
{
    public class ScoreController : MonoBehaviour
    {
        //This line would be replaced by DI
        [SerializeField] private ScoreViewer scoreViewer;

        private int totalPts = 0;

        public void AddThrowButtonListener(UnityAction call)
        {
            scoreViewer.RollButtonClicked.AddListener(call);
        }

        public void UpdateScore(int score)
        {
            totalPts += score;
            scoreViewer.UpdateScore(score, totalPts);
        }

        public void SetRollingText()
        {
            scoreViewer.SetRollingText();
        }
    }
}