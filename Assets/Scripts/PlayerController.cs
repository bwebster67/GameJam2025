using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerMoveSpeed = 6f;
    public float playerHealth = 100f;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;


    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player {damage} took damage! New health: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("Player has died.");
        }
    }

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        transform.Translate(moveInput * Time.deltaTime * playerMoveSpeed);
    }
}
