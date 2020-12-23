using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequencePlayer : MonoBehaviour
{
    [SerializeField] float framesPerSecond = 10.0f;
    [SerializeField] TutorialContainer levelTutorial;
    [SerializeField] RawImage image;

    Texture[] frames;

    int index = 0;

    void Update()
    {
        frames = levelTutorial.GetFrames();

        int index = (int)(Time.time * framesPerSecond);
        index = index % frames.Length;
        image.texture = frames[index];
    }

    private void OnEnable()
    {
        index = 0;
    }

    public void SetTutorial(TutorialContainer tutorial)
    {
        levelTutorial = tutorial;
    }
}
