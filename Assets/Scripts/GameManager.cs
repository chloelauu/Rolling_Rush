using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Won, Lost }
    public GameState State { get; private set; } = GameState.Playing;

    public GameObject completeLevelUI;
    public LevelSettings settings;
    public TMP_Text levelText;

    public void Start()
    {
        levelText.text = settings.levelName;
    }

    public void CompleteLevel()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;
        completeLevelUI.SetActive(true);
        levelText.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        Invoke(nameof(Restart), settings.restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}