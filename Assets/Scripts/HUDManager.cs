using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public void ExitGame()
    {
        SceneManager.LoadScene("StartScene"); 
    }
}
