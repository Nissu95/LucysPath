using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip mainMenuClip;
    [SerializeField] AudioClip onGameClip;
    [SerializeField] AudioClip portalClip;
    [SerializeField] AudioClip reversePortalClip;
    [SerializeField] AudioClip pickUpClip;
    [SerializeField] AudioClip pickUpStarClip;
    [SerializeField] AudioClip winClip;

    [SerializeField] AudioClip[] nyanClips;

    bool mute = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;

        audioSource.clip = mainMenuClip;
        audioSource.Play();
    }

    public void ChangeToGame()
    {
        if (audioSource.clip != onGameClip)
        {
            audioSource.clip = onGameClip;
            audioSource.Play();
        }
    }

    public void ChangeToMenu()
    {
        if (audioSource.clip != mainMenuClip)
        {
            audioSource.clip = mainMenuClip;
            audioSource.Play();
        }
    }

    public void Nyan()
    {
        audioSource.PlayOneShot(nyanClips[Random.Range(0, nyanClips.Length)]);
    }

    public void PlayPortalClip()
    {
        audioSource.PlayOneShot(portalClip);
    }

    public void PortalReverseClip()
    {
        audioSource.PlayOneShot(reversePortalClip);
    }

    public void PickUpClip()
    {
        audioSource.PlayOneShot(pickUpClip);
    }

    public void PickUpStarClip()
    {
        audioSource.PlayOneShot(pickUpStarClip);
    }

    public void Mute()
    {
        mute = !mute;

        if (mute)
            audioSource.volume = 0;
        else
            audioSource.volume = 1;
    }

    public void WinClip()
    {
        audioSource.PlayOneShot(winClip);
        ChangeToMenu();
    }
}
