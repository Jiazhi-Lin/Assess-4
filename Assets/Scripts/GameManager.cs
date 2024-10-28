using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentScore;
    public float currentTime;

    public void GameOver()
    {
        StopAllMovement();
        SaveScoreIfHighScore();
        ShowGameOverText();
        Invoke("ReturnToStartScene", 3f);
    }

    private void StopAllMovement()
    {
        
    }

    private void SaveScoreIfHighScore()
    {
        int previousHighScore = PlayerPrefs.GetInt("HighScore", 0);
        float previousHighScoreTime = PlayerPrefs.GetFloat("HighScoreTime", 0f);

        if (currentScore > previousHighScore ||
            (currentScore == previousHighScore && currentTime < previousHighScoreTime))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.SetFloat("HighScoreTime", currentTime);
            PlayerPrefs.Save();
        }
    }

    private void ShowGameOverText()
    {
  
    }

    private void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
