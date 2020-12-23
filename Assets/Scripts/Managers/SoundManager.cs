using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Sprite muteIcon;
    [SerializeField] Sprite unmuteIcon;

    [SerializeField] Image[] allMuteIcons;

    bool mute = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;

        audioSource.clip = mainMenuClip;
        mute = (PlayerPrefs.GetInt("Mute") != 0);
        SetVolume();
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
        audioSource.PlayOneShot(nyanClips[Random.Range(0, nyanClips.Length)], 0.5f);
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
        PlayerPrefs.SetInt("Mute", (mute ? 1 : 0));
        SetVolume();
    }

    void SetVolume()
    {
        if (mute)
        {
            audioSource.volume = 0;

            for (int i = 0; i < allMuteIcons.Length; i++)
                allMuteIcons[i].sprite = muteIcon;
        }
        else
        {
            audioSource.volume = 1;

            for (int i = 0; i < allMuteIcons.Length; i++)
                allMuteIcons[i].sprite = unmuteIcon;
        }
    }

    public void WinClip()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(winClip);
    }

    public float GetWinClipDuration()
    {
        return winClip.length;
    }
}
