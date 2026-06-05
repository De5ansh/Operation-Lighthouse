using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    Animator anim;
    
    [Header("Movement Settings")]
    public float baseSpeed = 5f; 

    [Header("Map Boundary Limits")]
    // Customize these numbers in the Inspector to fit your map layout perfectly!
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
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        
        // 1. Calculate the intended next movement position
        Vector3 movement = new Vector3(direction.x, 0, direction.y) * baseSpeed * Time.deltaTime; 
        transform.position += movement;

        // 2. BOUNDARY CLAMPING LOGIC
        // This forces the player's position to strictly stay within your min and max boxes!
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

        // 3. Apply the clamped position back to the player transform (keeping Y exactly where it is)
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

        // Handle animation parameters
        if (anim != null)
        {
            anim.SetFloat("Speed", direction.magnitude);
        }

        // Handle rotational direction look loops
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