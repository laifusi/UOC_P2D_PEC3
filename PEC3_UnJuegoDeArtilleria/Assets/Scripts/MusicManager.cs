using System.Collections;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private AudioSource audioSource; //AudioSource component

    /// <summary>
    /// Awake method to get the AudioSource component
    /// </summary>
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Method to be called from other classes to pause the music
    /// </summary>
    public void StopMusic()
    {
        StartCoroutine(nameof(StopAndReset));
    }

    /// <summary>
    /// Coroutine to pause the music for 2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopAndReset()
    {
        audioSource.Stop();
        yield return new WaitForSeconds(1);
        audioSource.Play();
    }
}
