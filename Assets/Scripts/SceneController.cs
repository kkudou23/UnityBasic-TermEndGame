using naichilab.EasySoundPlayer.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class SceneController : MonoBehaviour {

    public void LoadTitleScene() {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadHowToScene() {
        SePlayer.Instance.Play(0);
        SceneManager.LoadScene("HowToScene");
    }

    public void LoadModeSelectScene() {
        SePlayer.Instance.Play(0);
        SceneManager.LoadScene("ModeSelectScene");
    }

    private void Update() {
        if (SceneManager.GetActiveScene().name.Equals("TitleScene"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                LoadModeSelectScene();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                LoadHowToScene();
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("HowToScene") || SceneManager.GetActiveScene().name.Equals("ResultScene"))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SePlayer.Instance.Play(0);
                SceneManager.LoadScene("ModeSelectScene");
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("ModeSelectScene"))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LoadNormalModeGame();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                LoadEndlessModeGame();
            }
        }
    }

    public void LoadNormalModeGame() {
        GameSettings.isEndlessMode = false;
        SceneManager.LoadScene("CountdownScene");
    }

    public void LoadEndlessModeGame() {
        GameSettings.isEndlessMode = true;
        SceneManager.LoadScene("CountdownScene");
    }
}
