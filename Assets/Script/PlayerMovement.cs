using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    private Vector2 horizontalMovement;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMovement.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
       float currentSpeed = Mathf.Abs(horizontalMovement.x);
        anim.SetFloat("Speed", currentSpeed);
        if (horizontalMovement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalMovement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
    }
}
