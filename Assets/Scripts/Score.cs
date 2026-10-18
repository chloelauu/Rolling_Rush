using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    Transform player;
    GameManager gameManager;
    public TMP_Text scoreText;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.State != GameManager.GameState.Playing) return;
        scoreText.text = player.position.z.ToString("0") + "<size=45%><color=#9EC3F0> m</color></size>";
    }
}