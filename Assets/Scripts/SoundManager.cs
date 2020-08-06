using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource background;
    [SerializeField] private AudioSource select;
    private bool isForward = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void ChangeState()
    {
        StopAllCoroutines();
        isForward = !isForward;
        background.pitch = 0;
        if (!isForward)
        {
            StartCoroutine(PlayBackward());
        }
        else
        {
            StartCoroutine(PlayForward());
        }
    }

    public void SetPitch()
    {
        background.pitch = 1;
    }

    private IEnumerator PlayBackward()
    {
        while (background.pitch>-1)
        {
            background.pitch -= 0.01f;
            yield return null;
        }
    }
    private IEnumerator PlayForward()
    {
        while (background.pitch < 1)
        {
            background.pitch += 0.01f;
            yield return null;
        }
    }
}
