using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerMoveSpeed = 6f;

    private Animator anim;
    public Rigidbody2D rb;
    [SerializeField] private Vector2 input;
    [SerializeField] private Vector2 lastMoveDirection;


    [SerializeField] private float moveX;
    [SerializeField] private float moveY;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
        ProcessInputs();
        Animate();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input.normalized * playerMoveSpeed;
    }

    void ProcessInputs()
    {
         moveX = Input.GetAxisRaw("Horizontal");
         moveY = Input.GetAxisRaw("Vertical");

        input = new Vector2(moveX, moveY);

        if (input != Vector2.zero)
        {
            lastMoveDirection = input;
        }
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);

        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }
}
