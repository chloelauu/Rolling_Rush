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
    public TMP_Text completeHeader;
    public TMP_Text completeTitle;
    public TMP_Text completeSubtitle;
    float crashDistance;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Time.timeScale = 1f;
        levelText.text = settings.levelName;
        bool hasHazard = settings.newHazard != "";
        hazardText.text = settings.newHazard;
        hazardText.gameObject.SetActive(hasHazard);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();

    #if UNITY_EDITOR
        // debug shortcuts (editor only, not included in builds)
        if (Input.GetKeyDown(KeyCode.F1)) CompleteLevel();   // instant level complete
        if (Input.GetKeyDown(KeyCode.F2)) EndGame();         // instant game over
        if (Input.GetKeyDown(KeyCode.F3)) ScreenCapture.CaptureScreenshot("Screenshot_" + System.DateTime.Now.Ticks + ".png", 2);   // screenshot
    #endif
    }

    public void HideHazard()
    {
        hazardText.gameObject.SetActive(false);
    }

    bool SaveBest(float distance, out float best)
    {
        string key = "Best_" + SceneManager.GetActiveScene().name;
        best = PlayerPrefs.GetFloat(key, 0f);
        if (distance <= best) return false;

        best = distance;
        PlayerPrefs.SetFloat(key, best);
        PlayerPrefs.Save();
        return true;
    }

    public void EndGame(bool fell = false)
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        AudioManager.Play(fell ? AudioManager.Sfx.Fall : AudioManager.Sfx.Crash);
        crashTitle.text = fell ? "LEFT THE SHEET" : "COLLISION DETECTED";
        crashDistance = player.position.z;
        Invoke(nameof(ShowGameOver), settings.restartDelay);
    }

    void ShowGameOver()
    {
        float distance = crashDistance;
        bool newBest = SaveBest(distance, out float best);

        reportHeader.text = "TEST REPORT · " + settings.levelName;
        distanceText.text = distance.ToString("0") + " m";
        bestText.text = best.ToString("0") + " m";
        newRecordStamp.SetActive(newBest);
        AudioManager.SetPaused(true);

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
        AudioManager.SetPaused(true);
        pauseButton.SetActive(false);
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;
        State = GameState.Playing;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);
        AudioManager.SetPaused(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioManager.SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.SetPaused(false);
        SceneManager.LoadScene("Menu");
    }


    public void CompleteLevel()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;
        AudioManager.Play(AudioManager.Sfx.Complete);
        SaveBest(player.position.z, out _);

        completeLevelUI.SetActive(true);
        levelText.transform.parent.gameObject.SetActive(false);
        hazardText.gameObject.SetActive(false);
        pauseButton.SetActive(false);
        scoreText.SetActive(false);
        AudioManager.SetPaused(true);
        completeHeader.text = "TEST REPORT · " + settings.levelName;

        if (settings.isFinalLevel)
        {
            completeTitle.text = "PROJECT APPROVED";
            completeSubtitle.text = "All 3 sheets complete · Thanks for testing";
            Invoke(nameof(LoadNextLevel), 4f);
        }
        else
        {
            Invoke(nameof(LoadNextLevel), 2.5f);
        }
    }

    void LoadNextLevel()
    {
        AudioManager.SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}