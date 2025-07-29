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

    public QuestionDataList questionDataList;
    
    private List<QuestionData> selectedQuestions;
    private int currentQuestionIndex = 0;
    private int[] difficultyCount = new int[5];
    private int[] correctCount = new int[5];
    private bool isLeftCorrect;
    private int questionCount = 3;
    private float startTime;

    public GameObject[] lifeArray = new GameObject[3];
    private int lifePoint = 3;

    void Start() {
        startTime = Time.time;
        selectedQuestions = GetRandomQuestions();
        ShowNextQuestion();
    }


    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            LeftButtonSelected();
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            RightButtonSelected();
        }
    }

    public void LeftButtonSelected() {
        CheckAnswer(isLeftCorrect);
    }

    public void RightButtonSelected() {
        CheckAnswer(!isLeftCorrect);
    }

    void CheckAnswer(bool isCorrect)
    {
        var currentQuestion = selectedQuestions[currentQuestionIndex];
        if (isCorrect) {
            correctCount[currentQuestion.difficulty] += 1;
        } else {
            lifeArray[lifePoint-1].SetActive(false);
            lifePoint--;
        }
        currentQuestionIndex++;
        ShowNextQuestion();
    }

    void ShowNextQuestion() {
        if (lifePoint == 0 || currentQuestionIndex >= questionCount) {
            ShowResult();
            return;
        }

        var question = selectedQuestions[currentQuestionIndex];
        questionText.text = question.questionText;

        difficultyCount[question.difficulty] += 1;

        int randomLeftRight = Random.Range(0, 2);
        if(randomLeftRight < 1) {
            leftButtonText.text = question.firstOption;
            rightButtonText.text = question.secondOption;
            isLeftCorrect = (question.correctAnswer == question.firstOption) ? true : false;
        } else {
            leftButtonText.text = question.secondOption;
            rightButtonText.text = question.firstOption;
            isLeftCorrect = (question.correctAnswer == question.secondOption) ? true : false;
        }
    }

    List<QuestionData> GetRandomQuestions() {
        var shuffledQuestion = new List<QuestionData>(questionDataList.questions);
        for (int i = 0; i < questionCount; i++) {
            int randomIndex = Random.Range(i, shuffledQuestion.Count);
            var tmp = shuffledQuestion[i];
            shuffledQuestion[i] = shuffledQuestion[randomIndex];
            shuffledQuestion[randomIndex] = tmp;
        }
        return shuffledQuestion.GetRange(0, questionCount);
    }

    void ShowResult()
    {
        ResultData.playTime = Time.time - startTime;

        for (int i = 0; i < 5; i++) {
            ResultData.difficultyCount[i] = difficultyCount[i];
            ResultData.correctCount[i] = correctCount[i];
        }

        if (lifePoint == 0) {
            ResultData.isGameOver = true;
        }

        SceneManager.LoadScene("ResultScene");

    }
}
