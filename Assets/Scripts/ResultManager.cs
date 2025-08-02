using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI systemText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI messageText;

    void Start()
    {
        systemText.text = "";
        resultText.text = "";

        for (int i = 0; i < ResultData.difficultyCount.Length; i++)
        {
            systemText.text += $"なんいど {i + 1} ({(i+1) * 100} P) * {ResultData.correctCount[i]} P\n";
            resultText.text += $"{((i + 1) * 100) * ResultData.correctCount[i]} P\n";
        }

        systemText.text += $"クリアタイム ボーナス";
        resultText.text += $"{ResultData.playTime:F2} びょう = {ResultData.bonusScore} P";

        finalScoreText.text = $"{ResultData.correctScore + ResultData.bonusScore} P";

        if (ResultData.playTime < 10f)
        {
            messageText.text = "じゅう びょう いないに クリア";
        }
        else if (ResultData.playTime < 30f)
        {
            messageText.text = "さんじゅう びょう いないに クリア";
        }
        else if (ResultData.playTime < 60f)
        {
            messageText.text = "ろくじゅう びょう いないに クリア";
        }
        else
        {
            messageText.text = "ろくじゅう びょう いじょう かかった";
        }
    }
}