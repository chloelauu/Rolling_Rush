using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    GameManager gameManager;
    float steer; // -1 = left, 0 = none, 1 = right

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        steer = 0f;

        // keyboard
        if (Input.GetKey("d")) steer += 1f;
        if (Input.GetKey("a")) steer -= 1f;

        // touch (or mouse click in the editor)
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f) steer -= 1f;
            else steer += 1f;
        }

        steer = Mathf.Clamp(steer, -1f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.State != GameManager.GameState.Playing) return;

        rb.AddForce(0, 0, gameManager.settings.forwardForce * Time.deltaTime);

        rb.AddForce(steer * gameManager.settings.sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);

        if (rb.position.y < -1f)
        {
            gameManager.EndGame(true);
        }
    }
}
