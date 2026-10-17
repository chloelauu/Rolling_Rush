using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Won, Lost }
    public GameState State { get; private set; } = GameState.Playing;

    public GameObject completeLevelUI;
    public LevelSettings settings;

    public void CompleteLevel()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;
        completeLevelUI.SetActive(true);
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