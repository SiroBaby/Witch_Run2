using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXsource;

    public AudioClip background;
    public AudioClip death;
    public AudioClip wallTouch;

    private void Start () {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX (AudioClip clip) {
        SFXsource.PlayOneShot(clip);
    }
    // public void Update () {
    //     musicSource.clip = background;
    //     musicSource.StopBackgroundMusic;
    // }
}