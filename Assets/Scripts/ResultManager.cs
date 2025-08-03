using TMPro;
using UnityEngine;
using static GameManager;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI systemText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
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

        if (!GameSettings.isEndlessMode)
        {
            systemText.text += $"クリアタイム ボーナス";
            resultText.text += $"{ResultData.playTime:F2} びょう = {ResultData.bonusScore} P";
        }
        else
        {
            systemText.text += $"せいかいすう ボーナス";
            resultText.text += $"{ResultData.correctCountTotal} もん せいかい = {ResultData.bonusScore} P";
        }

        int finalScore = ResultData.correctScore + ResultData.bonusScore;

        string highScoreKey = GameSettings.isEndlessMode ? "HIGH_SCORE_ENDLESS" : "HIGH_SCORE";

        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        finalScoreText.text = $"{finalScore} P";

        if(finalScore > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, finalScore);
            PlayerPrefs.Save();
            highScoreText.text = "ハイスコア\nこうしん！";
        }
        else
        {
            highScoreText.text = $"ハイスコア\n{highScore} P";
        }

        if (ResultData.correctCountTotal == 0)
        {
            messageText.text = "ぜんもん ふせいかい じゃ ボーナスは ナシ じゃ";
        }
        else if (ResultData.playTime < 10f)
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