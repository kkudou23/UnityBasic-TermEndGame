using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public void LoadTitleScene() {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadHowToScene() {
        SceneManager.LoadScene("HowToScene");
    }

    public void LoadGameScene() {
        SceneManager.LoadScene("GameScene");
    }
}
