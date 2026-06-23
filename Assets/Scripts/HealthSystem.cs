using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class HealthSystem : MonoBehaviour
{
    public float Health; //health of the character
    public Slider HealthBar; //health bar reference
    public Image finalImage; //image reference for the final screen
    public static bool isragdoll = false;
    public void takedamage(float damage) //function responsible for lowering health;
    {
        Health -= damage;
        Debug.Log("Health: " + Health);
        HealthBar.value = Health;
        if (Health == 0) 
        {
            StartCoroutine(restart());
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("isdead"); //triggers the death animation1
            isragdoll = true; //sets the ragdoll state to true;
        }
    }

    IEnumerator restart()
    {
        yield return new WaitForSeconds(4.12f); //waiting for the ragdoll animation to play;
        finalImage.enabled = true;
        yield return new WaitForSeconds(2f); //waiting for 1 second before restarting the game;
        Application.OpenURL("https://mehran13579.itch.io/boxing-unity-25d");
    }
    
    
}
