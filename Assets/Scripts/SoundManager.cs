using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    #endregion

    [SerializeField] private AudioSource background = null;
    [SerializeField] private AudioSource effects = null;
    [SerializeField] private AudioClip[] clickSounds = null,selectSounds=null,slideSounds=null,rewindSounds=null
        , winSounds = null, loseSounds = null, scoreSounds = null;

    private void Start()
    {
        background.volume = PlayerPrefs.GetFloat("BackgroundVolume", 0.5f);
        effects.volume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
    }
    public void SetBackgroundVolume(float value)
    {
        background.volume = value;
        PlayerPrefs.SetFloat("BackgroundVolume", background.volume);
    }
    public void SetEffectsVolume(float value)
    {
        effects.volume = value;
        PlayerPrefs.SetFloat("EffectVolume", effects.volume);
    }
    public void Click()
    {
        effects.PlayOneShot(clickSounds[Random.Range(0,clickSounds.Length)]);
    }
    public void Select()
    {
        effects.PlayOneShot(selectSounds[Random.Range(0,selectSounds.Length)]);
    }

    public void Win()
    {
        effects.PlayOneShot(winSounds[Random.Range(0, winSounds.Length)]);
    }
    public void Lose()
    {
        effects.PlayOneShot(loseSounds[Random.Range(0, loseSounds.Length)]);
    }
    public void Score()
    {
        effects.PlayOneShot(scoreSounds[Random.Range(0, scoreSounds.Length)]);
    }
    public void Slide()
    {
        effects.PlayOneShot(slideSounds[Random.Range(0, slideSounds.Length)]);
    }
    public void ChangeState(GameState state)
    {
        transform.DOKill();
        background.pitch = 0;
        switch (state)
        {
            case GameState.Forward:
                effects.Stop();
                PitchChange(1f, 0.75f);
                break;
            case GameState.Backward:
                effects.PlayOneShot(rewindSounds[Random.Range(0, rewindSounds.Length)]);
                PitchChange(-1f,0.75f);
                break;
        }
    }
    private void PitchChange(float value, float duration)
    {
        background.DOPitch(value, duration).OnKill(() => background.pitch = value);
    }
}
