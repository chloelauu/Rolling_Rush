using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Paused, Won, Lost }
    public GameState State { get; private set; } = GameState.Playing;

    public GameObject completeLevelUI;
    public GameObject pauseUI;
    public GameObject pauseButton;
    public LevelSettings settings;
    public TMP_Text levelText;
    public TMP_Text hazardText;
    public Transform player;
    public GameObject gameOverUI;
    public TMP_Text distanceText;
    public TMP_Text bestText;
    public TMP_Text pauseTimeText;
    public GameObject scoreText;
    public TMP_Text crashTitle;
    public TMP_Text reportHeader;
    public GameObject newRecordStamp;
    float crashDistance;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Time.timeScale = 1f;
        levelText.text = settings.levelName;
        bool hasHazard = settings.newHazard != "";
        hazardText.text = settings.newHazard;
        hazardText.gameObject.SetActive(hasHazard);
        if (hasHazard) Invoke(nameof(HideHazard), 4f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void HideHazard()
    {
        hazardText.gameObject.SetActive(false);
    }

    public void EndGame(bool fell = false)
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        crashTitle.text = fell ? "LEFT THE SHEET" : "COLLISION DETECTED";
        crashDistance = player.position.z;
        Invoke(nameof(ShowGameOver), settings.restartDelay);
    }

    void ShowGameOver()
    {
        float distance = crashDistance;
        
        string key = "Best_" + SceneManager.GetActiveScene().name;
        float best = PlayerPrefs.GetFloat(key, 0f);
        bool newBest = distance > best;

        if (newBest)
        {
            best = distance;
            PlayerPrefs.SetFloat(key, best);
            PlayerPrefs.Save();
        }

        reportHeader.text = "TEST REPORT · " + settings.levelName;
        distanceText.text = distance.ToString("0") + " m";
        bestText.text = best.ToString("0") + " m";
        newRecordStamp.SetActive(newBest);

        pauseButton.SetActive(false);
        levelText.transform.parent.gameObject.SetActive(false);
        scoreText.SetActive(false);
        hazardText.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void TogglePause()
    {
        if (State == GameState.Playing) Pause();
        else if (State == GameState.Paused) Resume();
    }

    public void Pause()
    {
        if (State != GameState.Playing) return;
        State = GameState.Paused;
        Time.timeScale = 0f;
        float t = Time.timeSinceLevelLoad;
        pauseTimeText.text = "T = " + ((int)(t / 60)).ToString("00") + ":" + (t % 60).ToString("00.0");
        pauseUI.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;
        State = GameState.Playing;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }


    public void CompleteLevel()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;
        completeLevelUI.SetActive(true);
        levelText.transform.parent.gameObject.SetActive(false);
        hazardText.gameObject.SetActive(false);
        pauseButton.SetActive(false);
    }
}