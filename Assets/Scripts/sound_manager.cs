using UnityEngine;
using UnityEngine.Rendering;

public class sound_manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource enemypunchaudiosource;  //separate audiosource since the enemy punch can be interrupted;
    public AudioClip player_punch;
    public AudioClip enemy_punch;
    public AudioClip gothit;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playsfx(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void playEnemyPunchSound(bool play) 
    {
        if (play)
        {
            enemypunchaudiosource.clip = enemy_punch;
            enemypunchaudiosource.Play(); 
        }
        else if (!play)
        {
            enemypunchaudiosource.Stop();      
        }
    }
}
