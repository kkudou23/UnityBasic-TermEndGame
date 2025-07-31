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

    public void LoadNormalModeGame() {
        GameSettings.isEndlessMode = false;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadEndlessModeGame() {
        GameSettings.isEndlessMode = true;
        SceneManager.LoadScene("GameScene");
    }
}
