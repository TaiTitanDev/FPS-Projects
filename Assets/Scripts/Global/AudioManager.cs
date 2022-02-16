using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource takeDameAudioSource;
    [SerializeField] private AudioSource playerJumpAudioSource;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource reloadAudioSource;
    [SerializeField] private AudioSource getAmmoAudioSource;
    [SerializeField] private AudioSource getCanteenAudioSource;

    public void OnTakeDameAudioSource()
    {
        takeDameAudioSource.Play();
    }
    public void OnPlayerJumpAudioSource()
    {
        playerJumpAudioSource.Play();
    }

    public void OnShootAudioSource()
    {
        shootAudioSource.Play();
    }
    public void OnReloadAudioSource()
    {
        reloadAudioSource.Play();
    }
    public void OnGetAmmoAudioSource()
    {
        getAmmoAudioSource.Play();
    }
    public void OnGetCanteenAudioSource()
    {
        getCanteenAudioSource.Play();
    }
}
