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
            systemText.text += $"�Ȃ񂢂� {i + 1} ({(i+1) * 100} P) * {ResultData.correctCount[i]} P\n";
            resultText.text += $"{((i + 1) * 100) * ResultData.correctCount[i]} P\n";
        }

        systemText.text += $"�N���A�^�C�� �{�[�i�X";
        resultText.text += $"{ResultData.playTime:F2} �т傤 = {ResultData.bonusScore} P";

        finalScoreText.text = $"{ResultData.correctScore + ResultData.bonusScore} P";

        if (ResultData.playTime < 10f)
        {
            messageText.text = "���イ �т傤 ���Ȃ��� �N���A";
        }
        else if (ResultData.playTime < 30f)
        {
            messageText.text = "���񂶂イ �т傤 ���Ȃ��� �N���A";
        }
        else if (ResultData.playTime < 60f)
        {
            messageText.text = "�낭���イ �т傤 ���Ȃ��� �N���A";
        }
        else
        {
            messageText.text = "�낭���イ �т傤 �����傤 ��������";
        }
    }
}