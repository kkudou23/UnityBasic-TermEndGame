using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

    async void Start()
    {
        systemText.text = "";
        resultText.text = "\n";

        for (int i = 0; i < ResultData.difficultyCount.Length; i++)
        {
            systemText.text += $"なんいど {i + 1} ({(i+1) * 100} P) * {ResultData.correctCount[i]} もん\n";
            resultText.text += $"{((i + 1) * 100) * ResultData.correctCount[i]} P\n";
        }

        if (!GameSettings.isEndlessMode)
        {
            if (ResultData.correctCountTotal == 20)
            {
                systemText.text += $"クリアタイム ボーナス\n";
                resultText.text += $"{ResultData.playTime:F2} びょう = {ResultData.bonusScore - 1000} P\n";
                systemText.text += $"ぜんもんせいかい ボーナス\n";
                resultText.text += $"1000 P\n";
            } else
            {
                systemText.text += $"クリアタイム ボーナス\n";
                resultText.text += $"{ResultData.playTime:F2} びょう = {ResultData.bonusScore} P\n";
            }
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

        messageText.text = GetResultMessage();

        if (GameSettings.isEndlessMode && finalScore >= 22222)
        {
            var token = this.GetCancellationTokenOnDestroy();

            await UniTask.Delay(TimeSpan.FromMinutes(5), cancellationToken: token);
            messageText.text = "こんな げーむを あそんでくれて ありがとう\n...ほんとに まって くれるとは";
        }
    }

    string GetResultMessage()
    {
        return GameSettings.isEndlessMode ? GetEndlessModeMessage() : GetNormalModeMessage();
    }

    string GetEndlessModeMessage()
    {
        var messageConditions = new List<(Func<bool> condition, string message)>
        {
            (() => ResultData.correctCountTotal == 0, "ぜんもん ふせいかい じゃ ボーナスは ナシ"),
            (() => ResultData.correctCountTotal <= 3, "もっと がんばれる はず"),
            (() => ResultData.correctCountTotal <= 5, "まだまだ"),
            (() => ResultData.correctCountTotal <= 10, "ぼちぼち…"),
            (() => ResultData.correctCountTotal <= 20, "いい かんじ"),
            (() => ResultData.correctScore + ResultData.bonusScore <= 8000, "わるく ないね"),
            (() => ResultData.correctScore + ResultData.bonusScore <= 10000, "なかなか いいね"),
            (() => ResultData.correctScore + ResultData.bonusScore <= 15000, "かなり いいね"),
            (() => ResultData.correctScore + ResultData.bonusScore <= 18000, "けっこう すごいね"),
            (() => ResultData.correctScore + ResultData.bonusScore <= 20000, "もう ひとこえ！"),
            (() => ResultData.correctScore + ResultData.bonusScore >= 22222, "えらいっ"),
            (() => true, "このメッセージが みれるのは おかしいよ"),
        };

        foreach (var (condition, message) in messageConditions)
        {
            if (condition()) return message;
        }

        return "";
    }

    string GetNormalModeMessage()
    {
        var messageConditions = new List<(Func<bool> condition, string message)>
        {
            (() => ResultData.correctCountTotal == 0, "ぜんもん ふせいかい じゃ ボーナスは ナシ"),
            (() => ResultData.correctCountTotal <= 3, "もっと がんばれる はず"),
            (() => ResultData.correctCountTotal <= 5, "まだまだ"),
            (() => ResultData.correctCountTotal <= 10, "ぼちぼち…"),
            (() => ResultData.correctCountTotal <= 15, "もう ひとこえ！"),
            (() => ResultData.correctCountTotal <= 19, "おしいっ"),
            (() => ResultData.correctCountTotal == 20, "ぜんもん せいかい おめでとう！"),
            (() => true, "このメッセージが みれるのは おかしいよ"),
        };

        foreach (var (condition, message) in messageConditions)
        {
            if (condition()) return message;
        }

        return "";
    }
}