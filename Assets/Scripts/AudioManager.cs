using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip swapSound;
    [SerializeField] AudioClip shuffleSound;
    [SerializeField] AudioClip matchSound;
    [SerializeField] AudioClip selectedSound;

    private void Start()
    {
        instance = GetComponent<AudioManager>();
    }

    public void PlaySwapSound()
    {
        audioSource.PlayOneShot(swapSound);
    }
    public void PlayShuffleSound()
    {
        audioSource.PlayOneShot(shuffleSound);
    }
    public void PlayMatchSound()
    {
        audioSource.PlayOneShot(matchSound);
    }
    public void PlaySelectedSound()
    {
        audioSource.PlayOneShot(selectedSound);
    }
}
