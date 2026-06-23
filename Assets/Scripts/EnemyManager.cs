using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    public Transform Player;
    public Animator animator;
    public CharacterController controller;
    public Collider punchcollider;
    public float attackrange = 0.7f;
    public float speed = 0.8f;
    bool fighting;
    bool retreating;
    public sound_manager sound;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(HealthSystem.isragdoll)   //stops the animations when ragdoll is activated;
        {
            StopAllCoroutines();
            return;
        }
        if (!fighting)
        {
            StartCoroutine(combatloop());
        }
        if (retreating)
        {
            controller.Move(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }
    IEnumerator combatloop()
    {
        fighting = true;
        while (Vector3.Distance(transform.position,Player.position) > attackrange)
        {
            controller.Move(new Vector3(-speed * Time.deltaTime, 0, 0));
            animator.SetBool("walking", true);
            yield return null;
        }
        animator.SetTrigger("attack");
        sound.playsfx(sound.enemy_punch); //plays enemy punch sound effect;
        yield return new WaitForSeconds(2f);
        retreating = true;
        yield return new WaitForSeconds(Random.Range(2, 5));
        animator.SetBool("walking", false);
        retreating = false;
        fighting = false;
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

