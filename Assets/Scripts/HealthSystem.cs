using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class HealthSystem : MonoBehaviour
{
    public float Health; //health of the character
    public Slider HealthBar; //health bar reference
    public Image finalImage; //image reference for the final screen
    public void takedamage(float damage) //function responsible for lowering health;
    {
        Health -= damage;
        Debug.Log("Health: " + Health);
        HealthBar.value = Health;
        if (Health == 0) 
        {
            StartCoroutine(restart());
        }
    }

    IEnumerator restart()
    {
        yield return new WaitForSeconds(1f); //waiting for the ragdoll animation to play;
        finalImage.enabled = true;
        yield return new WaitForSeconds(2f); //waiting for 1 second before restarting the game;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reloading the current scene
    }
    
    
}
