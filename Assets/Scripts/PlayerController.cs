using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction pauseAction;
    Animator anim;

    [Header("Pause UI Connection")]
    public PauseMenu pauseMenuScript;

    [Header("Movement Settings")]
    public float baseSpeed = 5f; 

    [Header("Map Boundary Limits")]
    public float minX = -20f;
    public float maxX = 20f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("UI Links")]
    public GameOver gameOverScript;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("move"); 
        pauseAction = playerInput.actions.FindAction("pause");
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (PauseMenu.isPaused)
        {
            // If they press Escape while paused, resume the match!
            if (pauseAction != null && pauseAction.WasPressedThisFrame())
            {
                pauseMenuScript.TogglePause();
            }
            return; 
        }

        MovePlayer();
        
        if (pauseAction != null && pauseAction.WasPressedThisFrame())
        {
            if (pauseMenuScript != null)
            {
                pauseMenuScript.TogglePause();
            }
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(direction.x, 0, direction.y) * baseSpeed * Time.deltaTime; 
        transform.position += movement;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

        if (anim != null)
        {
            anim.SetFloat("Speed", direction.magnitude);
        }

        if (direction.magnitude > 0)
        {
            Vector3 targetDir = new Vector3(direction.x, 0, direction.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * 12f);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            Destroy(other.gameObject);
            PlayerGameOver();
        }
    } 

    public void PlayerGameOver()
    {
        if (gameOverScript != null)
        {
            gameOverScript.DisplayGameOverScreen("The slimes have killed you!");
        } 
        else
        {
            Time.timeScale = 0f;
        }
    }
}