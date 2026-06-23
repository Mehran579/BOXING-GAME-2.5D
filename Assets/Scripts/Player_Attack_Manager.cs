using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Attack_Manager : MonoBehaviour
{
    public Animator animator;
    public Collider punchcollider;
    public float Punchcooldown;
    bool isattacking = false;
    void Start()
    {
        animator = GetComponent<Animator>(); // reference to player's animator component
        DisablePunchCollision(); //disable punch collision at start
    }

    public void OnAttack(InputAction.CallbackContext context) 
    {
        if (!isattacking)
        {
            animator.SetTrigger("IsAttacking"); //triggers attack animation
            StartCoroutine(attackcooldown()); //adds an attack cooldown to prevent spamming attacks
        }
    }

    IEnumerator attackcooldown()
    {
        isattacking = true;
        yield return new WaitForSeconds(Punchcooldown); // wait till cooldown before allowing another attack
        isattacking = false;
    }
    public void EnablePunchCollision() //enable collision whille attacking
    {
        punchcollider.enabled = true;
    }
    public void DisablePunchCollision() //disable collision whille not attacking
    {
        punchcollider.enabled = false;
    }
}
