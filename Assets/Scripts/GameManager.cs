using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject leftButton;
    public TextMeshProUGUI leftButtonText;
    public GameObject rightButton;
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI questionNumberText;

    public QuestionDataList questionDataList;

    private List<QuestionData> selectedQuestions;
    private int currentQuestionIndex = 0;
    private int[] difficultyCount = new int[5];
    private int[] correctCount = new int[5];
    private bool isLeftCorrect;
    private int questionCount;
    private float startTime;

    public GameObject[] lifeArray = new GameObject[3];
    private int lifePoint = 3;

    public static class GameSettings
    {
        public static bool isEndlessMode = false;
    }

    void Start()
    {
        startTime = Time.time;

        if (!GameSettings.isEndlessMode)
        {
            questionCount = 3;
            selectedQuestions = GetRandomQuestions(questionCount);
        }
        else
        {
            questionCount = questionDataList.questions.Count;
            selectedQuestions = GetRandomQuestions(questionCount);
        }

        ShowNextQuestion();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftButtonSelected();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightButtonSelected();
        }
    }

    public void LeftButtonSelected()
    {
        CheckAnswer(isLeftCorrect);
    }

    public void RightButtonSelected()
    {
        CheckAnswer(!isLeftCorrect);
    }

    void CheckAnswer(bool isCorrect)
    {
        var currentQuestion = selectedQuestions[currentQuestionIndex];
        if (isCorrect)
        {
            correctCount[currentQuestion.difficulty]++;
        }
        else
        {
            lifePoint--;
            if (lifePoint >= 0 && lifePoint < lifeArray.Length)
            {
                lifeArray[lifePoint].SetActive(false);
            }
        }
        currentQuestionIndex++;
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (lifePoint == 0 || (!GameSettings.isEndlessMode && currentQuestionIndex >= questionCount))
        {
            ShowResult();
            return;
        }

        if (GameSettings.isEndlessMode && currentQuestionIndex >= selectedQuestions.Count)
        {
            var newQuestions = GetRandomQuestions(questionDataList.questions.Count);
            selectedQuestions.AddRange(newQuestions);
        }

        var question = selectedQuestions[currentQuestionIndex];
        questionText.text = question.questionText;

        difficultyCount[question.difficulty]++;

        int randomLeftRight = Random.Range(0, 2);
        if (randomLeftRight == 0)
        {
            leftButtonText.text = question.firstOption;
            rightButtonText.text = question.secondOption;
            isLeftCorrect = (question.correctAnswer == question.firstOption);
        }
        else
        {
            leftButtonText.text = question.secondOption;
            rightButtonText.text = question.firstOption;
            isLeftCorrect = (question.correctAnswer == question.secondOption);
        }

        if (GameSettings.isEndlessMode)
        {
            questionNumberText.text = (currentQuestionIndex + 1).ToString();
        }
        else
        {
            questionNumberText.text = $"{currentQuestionIndex + 1} / {questionCount}";
        }
    }

    List<QuestionData> GetRandomQuestions(int count)
    {
        var shuffledQuestions = new List<QuestionData>(questionDataList.questions);

        for (int i = 0; i < shuffledQuestions.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledQuestions.Count);
            var tmp = shuffledQuestions[i];
            shuffledQuestions[i] = shuffledQuestions[randomIndex];
            shuffledQuestions[randomIndex] = tmp;
        }

        if (count > shuffledQuestions.Count)
        {
            count = shuffledQuestions.Count;
        }

        return shuffledQuestions.GetRange(0, count);
    }

    void ShowResult()
    {
        ResultData.playTime = Time.time - startTime;

        for (int i = 0; i < difficultyCount.Length; i++)
        {
            ResultData.difficultyCount[i] = difficultyCount[i];
            ResultData.correctCount[i] = correctCount[i];
        }

        if (lifePoint == 0) {
            ResultData.isGameOver = true;
        }

        SceneManager.LoadScene("ResultScene");
    }
}
