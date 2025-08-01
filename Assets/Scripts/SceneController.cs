using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class SceneController : MonoBehaviour {

    public void LoadTitleScene() {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadHowToScene() {
        SceneManager.LoadScene("HowToScene");
    }

    public void LoadModeSelectScene() {
        SceneManager.LoadScene("ModeSelectScene");
    }

    private void Update() {
        if (SceneManager.GetActiveScene().name.Equals("HowToScene"))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
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
