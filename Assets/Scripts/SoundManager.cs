using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource background = null;
    [SerializeField] private AudioSource rewind = null;
    [SerializeField] private AudioClip clickSound = null;
    private bool isForward = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Click()
    {
        rewind.Stop();
        rewind.PlayOneShot(clickSound);
    }

    public void ChangeState()
    {
        StopAllCoroutines();
        isForward = !isForward;
        background.pitch = 0;
        if (!isForward)
        {
            rewind.Play();
            StartCoroutine(PlayBackward());
        }
        else
        {
            rewind.Stop();
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
