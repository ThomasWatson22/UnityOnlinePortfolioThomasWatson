using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Attack()
    {
        animator.SetTrigger("Attacking");
    }
}
