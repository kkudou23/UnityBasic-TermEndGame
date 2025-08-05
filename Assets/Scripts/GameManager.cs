using Cysharp.Threading.Tasks;
using naichilab.EasySoundPlayer.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI questionNumberText;
    public Slider timeLimitSlider;
    public GameObject pausePanel;
    public Image birdImage;

    public QuestionDataList questionDataList;

    private List<QuestionData> selectedQuestions;
    private int currentQuestionIndex = 0;
    private int[] difficultyCount = new int[5];
    private int[] correctCount = new int[5];
    private int correctCountTotal;
    private bool isLeftCorrect;
    private int questionCount;

    private float startTime;
    private float timeLimit = 5f;
    private float minTimeLimit = 1f;
    private Coroutine timerCoroutine;

    public GameObject[] lifeArray = new GameObject[3];
    private int lifePoint = 3;

    private int score = 0;
    private float bonus = 0;

    private bool isPaused = false;
    private CancellationTokenSource timerCancellationTokenSource;
    private float elapsedTime = 0f;

    public static class GameSettings
    {
        public static bool isEndlessMode = false;
    }

    void Start()
    {
        //PlayerPrefs.DeleteAll();


        if (questionDataList == null || questionDataList.questions == null || questionDataList.questions.Count == 0)
        {
            selectedQuestions = new List<QuestionData>();
            return;
        }

        startTime = Time.time;

        if (!GameSettings.isEndlessMode)
        {
            questionCount = 20;
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
        if (isPaused)
        {
            return;
        }

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
        if (selectedQuestions == null || currentQuestionIndex >= selectedQuestions.Count) return;
        CheckAnswer(isLeftCorrect);
    }

    public void RightButtonSelected()
    {
        if (selectedQuestions == null || currentQuestionIndex >= selectedQuestions.Count) return;
        CheckAnswer(!isLeftCorrect);
    }

    void CheckAnswer(bool isCorrect)
    {
        if (selectedQuestions == null || currentQuestionIndex >= selectedQuestions.Count) return;

        var currentQuestion = selectedQuestions[currentQuestionIndex];
        if (isCorrect)
        {
            SePlayer.Instance.Play(0);
            score += (currentQuestion.difficulty + 1) * 100;
            correctCount[currentQuestion.difficulty]++;
            correctCountTotal++;
        }
        else
        {
            SePlayer.Instance.Play(1);

            for (int i = 0; i < lifePoint; i++)
            {
                var animator = lifeArray[i].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("HeartShake");
                }
            }

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
        if (timeLimitSlider != null)
        {
            timeLimitSlider.value = 1f;
        }
        elapsedTime = 0f;

        timerCancellationTokenSource?.Cancel();
        timerCancellationTokenSource?.Dispose();

        timerCancellationTokenSource = new CancellationTokenSource();
        QuestionTimer().Forget();

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
        if (questionText != null)
        {
            questionText.text = question.questionText;
        }

        difficultyCount[question.difficulty]++;

        if (leftButtonText != null && rightButtonText != null)
        {
            int randomLeftRight = UnityEngine.Random.Range(0, 2);
            if (randomLeftRight == 0)
            {
                leftButtonText.text = question.correctOption;
                rightButtonText.text = question.wrongOption;
                isLeftCorrect = true;
            }
            else
            {
                leftButtonText.text = question.wrongOption;
                rightButtonText.text = question.correctOption;
                isLeftCorrect = false;
            }
        }

        if (questionNumberText != null)
        {
            if (GameSettings.isEndlessMode)
            {
                questionNumberText.text = (currentQuestionIndex + 1).ToString();
            }
            else
            {
                questionNumberText.text = $"{currentQuestionIndex + 1} / {questionCount}";
            }
        }

        birdImage.GetComponent<Animator>().SetTrigger("birdJump");

        timeLimit -= 0.1f;
        if (GameSettings.isEndlessMode && timeLimit < minTimeLimit)
        {
            timeLimit = minTimeLimit;
        }
    }

    List<QuestionData> GetRandomQuestions(int count)
    {
        var shuffledQuestions = new List<QuestionData>();
        if (questionDataList != null)
        {
            shuffledQuestions = new List<QuestionData>(questionDataList.questions);
        }

        for (int i = 0; i < shuffledQuestions.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, shuffledQuestions.Count);
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
        ResultData.correctCountTotal = correctCountTotal;

        if (ResultData.correctCountTotal != 0)
        {
            if (!GameSettings.isEndlessMode)
            {
                float maxTime = 60f;
                bonus = Mathf.Max(0, (maxTime - ResultData.playTime)) * 10;

                if (correctCountTotal == 20)
                {
                    bonus += 1000;
                }
            }
            else
            {
                bonus = ResultData.correctCountTotal * 50f;
            }
            ResultData.bonusScore = Mathf.RoundToInt(bonus);
        }

        ResultData.correctScore = score;

        if (lifePoint == 0)
        {
            ResultData.isGameOver = true;
        }

        SceneManager.LoadScene("ResultScene");
    }

    private async UniTask QuestionTimer()
    {
        timerCancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = timerCancellationTokenSource.Token;

        try
        {
            while (elapsedTime < timeLimit)
            {
                if (isPaused)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    continue;
                }

                elapsedTime += Time.deltaTime;
                if(timeLimitSlider != null)
                {
                    timeLimitSlider.value = 1f - (elapsedTime / timeLimit);
                }

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }

        SePlayer.Instance.Play(1);

        for (int i = 0; i < lifePoint; i++)
        {
            var animator = lifeArray[i].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("HeartShake");
            }
        }

        lifePoint--;
        if (lifeArray != null && lifePoint >= 0 && lifePoint < lifeArray.Length)
        {
            lifeArray[lifePoint].SetActive(false);
        }

        currentQuestionIndex++;

        if (lifePoint == 0 || (!GameSettings.isEndlessMode && currentQuestionIndex >= questionCount))
        {
            ShowResult();
        }
        else
        {
            ShowNextQuestion();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            return;
        }
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        SePlayer.Instance.Play(2);
    }

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        SePlayer.Instance.Play(2);
    }
    private void OnDestroy()
    {
        timerCancellationTokenSource?.Cancel();
        timerCancellationTokenSource?.Dispose();
    }
}