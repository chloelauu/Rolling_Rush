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
    public Transform player;
    public GameObject gameOverUI;
    public TMP_Text distanceText;
    public TMP_Text bestText;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Time.timeScale = 1f;
        levelText.text = settings.levelName;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void EndGame()
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        Invoke(nameof(ShowGameOver), settings.restartDelay);
    }

    void ShowGameOver()
    {
        float distance = player.position.z;

        string key = "Best_" + SceneManager.GetActiveScene().name;
        float best = PlayerPrefs.GetFloat(key, 0f);
        bool newBest = distance > best;

        if (newBest)
        {
            best = distance;
            PlayerPrefs.SetFloat(key, best);
            PlayerPrefs.Save();
        }

        distanceText.text = "Distance: " + distance.ToString("0");
        bestText.text = newBest ? "New best: " + best.ToString("0") + "!" : "Best: " + best.ToString("0");

        pauseButton.SetActive(false);
        levelText.gameObject.SetActive(false);
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
        levelText.gameObject.SetActive(false);
        pauseButton.SetActive(false);
    }
}