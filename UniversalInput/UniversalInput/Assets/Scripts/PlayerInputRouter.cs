using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    PlayerMover playerMover;
    PlayerAttacker playerAttacker;

    private void Start()
    {
        playerMover = GetComponent<PlayerMover>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        direction.y = 0;
        direction.Normalize();
        playerMover.Move(direction);
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (!context.started)
            return;

        playerMover.Jump();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        bool isAttacking = context.ReadValueAsButton();

        if(isAttacking)
            playerAttacker.Attack();    
    }
}
