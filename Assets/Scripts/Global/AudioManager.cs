using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource playerWallAudioSource;
    [SerializeField] private AudioSource playerJumpAudioSource;

    public void OnPlayerWallAudioSource()
    {
        playerWallAudioSource.Play();
    }
    public void OnPlayerJumpAudioSource()
    {
        playerJumpAudioSource.Play();
    }
}
