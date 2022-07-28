using System;
using TMPro;
using UnityEngine;

public class FacePoints : MonoBehaviour
{
    [SerializeField] private TextMeshPro pointsText;
    [SerializeField] private int points;
    public int Points => points;

    public void Init(int points)
    {
        this.points = points;
    }

    private void OnValidate()
    {
        pointsText.text = points.ToString();
        SetSpecialTextProperties();
    }

    private void SetSpecialTextProperties()
    {
        if (points == 6 || points == 9)
        {
            pointsText.fontStyle = FontStyles.Underline;
        }
    }
}
