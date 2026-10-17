using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    GameManager gameManager;
    
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        gameManager.CompleteLevel();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
}
