using UnityEngine;

public class PunchDamage : MonoBehaviour //responsible for punch damage and contact;
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding");
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            HealthSystem healthsystem = other.GetComponent<HealthSystem>(); //reference to enemy's health;
            healthsystem.takedamage(1f); //calling the take damage function and passing the damage value;
        }
    }
}
