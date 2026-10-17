using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    GameManager gameManager;

        void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.State != GameManager.GameState.Playing) return;

        rb.AddForce(0, 0, gameManager.settings.forwardForce * Time.deltaTime);

        if (Input.GetKey("d"))
        {
            rb.AddForce(gameManager.settings.sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-gameManager.settings.sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (rb.position.y < -1f)
        {
            gameManager.EndGame();
        }
    }
}
