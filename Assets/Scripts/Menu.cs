using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Text bestValue;
    public GameObject quitButton;


    void Start()
    {
        float best = Mathf.Max(
            PlayerPrefs.GetFloat("Best_DodgeZone", 0f),
            PlayerPrefs.GetFloat("Best_DodgeZoneLvl2", 0f),
            PlayerPrefs.GetFloat("Best_DodgeZoneLvl3", 0f));

        bestValue.text = best > 0f ? best.ToString("0") + " m" : "--";
// inside Start(), add:
#if UNITY_WEBGL
    quitButton.SetActive(false);   // web pages can't close themselves, so hide it
#endif
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
