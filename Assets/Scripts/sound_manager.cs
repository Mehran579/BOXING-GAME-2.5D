using UnityEngine;

public class sound_manager : MonoBehaviour
{
    public AudioSource audioSource;
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
}
