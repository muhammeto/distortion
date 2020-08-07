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
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
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
            PlayForward();
        }
        rewind.PlayOneShot(clickSound);
    }

    public void ChangeState()
    {
        isForward = !isForward;
        background.pitch = 0;
        if (!isForward)
        {
            rewind.Play();
            PlayBackward();
        }
        else
        {
            rewind.Stop();
            PlayForward();
        }
    }

    public void SetPitch()
    {
        background.pitch = 1;
    }

    private void PlayBackward()
    {
        background.DOPitch(-1f, 0.75f);
    }
    private void PlayForward()
    {
        background.DOPitch(1f, 0.75f);
    }
}
