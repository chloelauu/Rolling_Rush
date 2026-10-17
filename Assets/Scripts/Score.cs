using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Transform player;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = player.position.z.ToString("0");
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
}
