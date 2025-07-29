using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        int totalCorrectCount = 0;
        resultText.text = $"�N���A�^�C�� : {ResultData.playTime:F2} �т傤\n";
        for (int i = 0; i < ResultData.difficultyCount.Length; i++) {
            resultText.text += $"�Ȃ񂢂�{i + 1} : {ResultData.difficultyCount[i]} ���񂿂イ {ResultData.correctCount[i]} ���񂹂�����\n";
            totalCorrectCount += ResultData.correctCount[i];
        }

        resultText.text += "\n";

        if (ResultData.playTime < 30f && totalCorrectCount >= 3)
        {
            resultText.text += "������񂹂�����";
        }
        else if (ResultData.playTime < 60f && totalCorrectCount >= 2)
        {
            resultText.text += "�ɂ��񂹂�����";
        }
        else if (totalCorrectCount >= 1)
        {
            resultText.text += "�������񂹂�����";
        }
        else
        {
            resultText.text += "�������ӂ�������";
        }
    }
}