using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public Text countdownText;
    public GameObject countdownPanel;
    public GameObject player;
    public AudioSource backgroundMusic;
    public GameTimer gameTimer;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        if (countdownPanel == null || countdownText == null || player == null || gameTimer == null)
        {
            Debug.LogError("One or more required references are not set in CountdownManager.");
            yield break;
        }

        countdownPanel.SetActive(true);
        string[] countdownMessages = { "3", "2", "1", "GO!" };

        for (int i = 0; i < countdownMessages.Length; i++)
        {
            countdownText.text = countdownMessages[i];
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "";
        countdownPanel.SetActive(false);
        EnablePlayerControl();
        StartBackgroundMusic();
        gameTimer.StartTimer();
    }

    private void EnablePlayerControl()
    {
        player.GetComponent<PacStudentController>().enabled = true;
    }

    private void StartBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
            backgroundMusic.loop = true;
        }
    }
}
