using naichilab.EasySoundPlayer.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 3f;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        float currentTime = countdownTime;

        while (currentTime > 0)
        {
            SePlayer.Instance.Play(3);
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
        }

        SceneManager.LoadScene("GameScene");
    }
}
