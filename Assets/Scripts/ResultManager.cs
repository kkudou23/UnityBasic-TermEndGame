using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        resultText.text = $"Time: {ResultData.playTime:F2} seconds\n";
        for (int i = 0; i < ResultData.correctCount.Length; i++) {
            resultText.text += $"Difficulty-{i}: {ResultData.correctCount[i]}\n";
        }
    }
}