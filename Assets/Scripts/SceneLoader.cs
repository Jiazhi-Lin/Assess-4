using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public Text highScoreText;
    public Text highScoreTimeText;

    void Start()
    {
        LoadHighScore();
    }

    private void LoadHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float highScoreTime = PlayerPrefs.GetFloat("HighScoreTime", 0f);

        highScoreText.text = "High Score: " + highScore;
        highScoreTimeText.text = "Best Time: " + FormatTime(highScoreTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Pacman");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Pacman"); 
    }
}
