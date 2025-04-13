using Unity.VisualScripting;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;

    private float moveSpeed = 1000;
    private float maxSpeed = 5.0f;
    private float jumpForce = 500.0f;

    private bool isGrounded = false;

    private bool isTuring = false;

    private SpriteRenderer spriteRenderer;

    private float currentSpeedNormal => Mathf.Abs(rb.linearVelocityX / maxSpeed);

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        AddMoveForce();
        SetAnimatorParamaters();
    }

    public void Move(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void AddMoveForce()
    {
        if (rb.linearVelocity.magnitude >= maxSpeed)
            return;

        if(spriteRenderer.flipX && rb.linearVelocityX > 0.05 && !isTuring)
        {
            isTuring = true;
            animator.SetTrigger("Turning");
        }
        else if (!spriteRenderer.flipX && rb.linearVelocityX < -0.05 && !isTuring)
        {
            isTuring = true;
            animator.SetTrigger("Turning");
        }

        rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void SetAnimatorParamaters()
    {
        animator.SetFloat("Speed", currentSpeedNormal);
        animator.SetFloat("Y Velocity", rb.linearVelocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void Jump()
    {
        if (!isGrounded)
            return;

        rb.AddForce(Vector2.up * jumpForce);
        animator.SetTrigger("Jump");
    }

    public void FlipX()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        isTuring = false;
    }
}
