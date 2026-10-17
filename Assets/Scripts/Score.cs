using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Transform player;
    GameManager gameManager;
    public Text scoreText;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.State != GameManager.GameState.Playing) return;
        scoreText.text = player.position.z.ToString("0");
    }
}