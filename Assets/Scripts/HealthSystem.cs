using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float Health; //health of the character
    public void takedamage(float damage) //function responsible for lowering health;
    {
        Health -= damage;
        Debug.Log("Health: " + Health);
    }
}
