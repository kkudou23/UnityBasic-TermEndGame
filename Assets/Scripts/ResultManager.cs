using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        int totalCorrectCount = 0;
        resultText.text = $"クリアタイム : {ResultData.playTime:F2} びょう\n";
        for (int i = 0; i < ResultData.difficultyCount.Length; i++) {
            resultText.text += $"なんいど{i + 1} : {ResultData.difficultyCount[i]} もんちゅう {ResultData.correctCount[i]} もんせいかい\n";
            totalCorrectCount += ResultData.correctCount[i];
        }

        resultText.text += "\n";

        if (ResultData.playTime < 30f && totalCorrectCount >= 3)
        {
            resultText.text += "さんもんせいかい";
        }
        else if (ResultData.playTime < 60f && totalCorrectCount >= 2)
        {
            resultText.text += "にもんせいかい";
        }
        else if (totalCorrectCount >= 1)
        {
            resultText.text += "いちもんせいかい";
        }
        else
        {
            resultText.text += "ぜんもんふせいかい";
        }
    }
}