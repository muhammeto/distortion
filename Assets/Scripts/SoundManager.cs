using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager _instance;
    public static SoundManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(_instance.gameObject);
    }

    [SerializeField] private AudioSource background = null;
    [SerializeField] private AudioSource rewind = null;
    [SerializeField] private AudioClip clickSound = null;
    private bool isForward = true;

    public void Click()
    {
        if (!isForward)
        {
            rewind.Stop();
            isForward = true;
            StartCoroutine(PlayForward());
        }
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
