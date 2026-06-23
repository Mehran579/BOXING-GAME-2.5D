using UnityEngine;

public class PunchDamage : MonoBehaviour //responsible for punch damage and contact;
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthSystem healthsystem = other.GetComponent<HealthSystem>(); //reference to enemy's health;
            healthsystem.takedamage(1f); //calling the take damage function and passing the damage value;
        }
    }
}
