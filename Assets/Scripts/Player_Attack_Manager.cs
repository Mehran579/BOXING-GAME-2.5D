using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Attack_Manager : MonoBehaviour
{
    public static Player_Attack_Manager instance;
    public sound_manager sound_Manager;
    public Animator animator;
    public Collider punchcollider;
    public Collider leftpunchcollider;
    public bool attacking;
    public float currentattack = 0f;
    public float combotimer=0.5f;
    private bool canCombo = true;
    float lastattacktime;
    public static bool isblocking;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>(); // reference to player's animator component
        DisablePunchCollision(); //disable punch collision at start
        leftDisablePunchCollision(); //disable left punch collision at start
    }

    public void OnAttack(InputAction.CallbackContext context) 
    {
        if (HealthSystem.isragdoll) return;
        if (context.performed)
        {
            attacking = true;
            Attack();
        }
    }

    void Attack()
    {
        if (currentattack != 0 && (Time.time - lastattacktime > combotimer))
        {
            currentattack = 0;
        }
        if (!canCombo) return;
        if (currentattack == 0)
        {
            animator.SetTrigger("IsAttacking");
            currentattack = 1;
        }
        else if (currentattack == 1)
        {
            animator.SetTrigger("2ndpunch");
            currentattack = 2;
        }
        else if (currentattack == 2)
        {
            animator.SetTrigger("3rdpunch");
            currentattack = 0;
        }

        lastattacktime = Time.time;
        StartCoroutine(ComboLock());
    }

    IEnumerator ComboLock()
    {
        canCombo = false;
        yield return new WaitForSeconds(0.4f);
        canCombo = true;
    }
    public void EnablePunchCollision() //enable collision whille attacking
    {
        punchcollider.enabled = true;
    }
    public void DisablePunchCollision() //disable collision whille not attacking
    {
        punchcollider.enabled = false;
    }
    public void leftEnablePunchCollision() //enable collision whille attacking
    {
        leftpunchcollider.enabled = true;
    }
    public void leftDisablePunchCollision() //disable collision whille not attacking
    {
        leftpunchcollider.enabled = false;
    }
    public void OnBlock(InputAction.CallbackContext context)
    {
        if(HealthSystem.isragdoll) return;
        if (context.performed)
        {
            isblocking = true;
            animator.SetBool("blocking",true);
        }
        if (context.canceled)
        {
            isblocking = false;
            animator.SetBool("blocking",false);
        }
    }
}
