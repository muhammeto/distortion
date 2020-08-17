using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI: MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel=null, levelPanel=null, optionsPanel=null,quitPanel=null;
    [SerializeField] private VolumeProfile postProcess = null;
    [SerializeField] private SelectorUI menuSelector = null, quitSelector = null, levelSelector = null,optionsSelector=null;
    [SerializeField] private Slider backgroundVolume, effectsVolume = null;
    private SelectorUI _currentSelector = null;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;

    private void Start()
    {
        postProcess.TryGet(out distortion);
        distortion.intensity.value = -0.5f;
        distortion.xMultiplier.value = 0;
        distortion.yMultiplier.value = 0;
        distortion.scale.value = 1;

        backgroundVolume.value = PlayerPrefs.GetFloat("BackgroundVolume", 0.5f);
        effectsVolume.value = PlayerPrefs.GetFloat("EffectVolume", 0.5f);

        ChangeSelector(menuSelector);
    }
    private void ChangeSelector(SelectorUI changeTo = null)
    {
        menuSelector.gameObject.SetActive(false);
        quitSelector.gameObject.SetActive(false);
        levelSelector.gameObject.SetActive(false);
        optionsSelector.gameObject.SetActive(false);
        
        if (changeTo != null)
        {
            SoundManager.Instance.Slide();
            changeTo.gameObject.SetActive(true);
            _currentSelector = changeTo;
        }
    }
    public void OnMouseEnterSelectable(RectTransform selectThis)
    {
        _currentSelector.Select(selectThis);
    } 
    public void OnPlayButtonClick()
    {
        SoundManager.Instance.Click();
        menuPanel.DOLocalMoveX(-2000, 0.5f);
        levelPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelector(levelSelector);
    }
    public void OnPlayBackButtonClick()
    {
        SoundManager.Instance.Click();
        ChangeSelector(menuSelector);
        levelPanel.DOLocalMoveX(-2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
    }
    public void OnOptionsButtonClick()
    {
        SoundManager.Instance.Click();
        ChangeSelector(optionsSelector);
        menuPanel.DOLocalMoveX(-2000, 0.5f);
        optionsPanel.DOLocalMoveX(-135, 0.5f);
    }
    public void OnOptionsBackButtonClick()
    {
        SoundManager.Instance.Click();
        optionsPanel.DOLocalMoveX(-2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelector(menuSelector);
    }
    public void OnQuitButtonClick()
    {
        SoundManager.Instance.Click();
        ChangeSelector(quitSelector);
        menuPanel.DOLocalMoveX(-2000, 0.5f);
        quitPanel.DOLocalMoveX(-135, 0.5f);
    }
    public void OnQuitBackButtonClick()
    {
        SoundManager.Instance.Click();
        ChangeSelector(menuSelector);
        quitPanel.DOLocalMoveX(-2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
    }
    public void OnQuitAcceptButtonClick()
    {
        SoundManager.Instance.Click();
        Application.Quit();
    }
    public void OnLevelButtonClick(string levelName)
    {
        SoundManager.Instance.Click();
        distortion.xMultiplier.value = 1;
        distortion.yMultiplier.value = 1;
        DistortionDecrease(-1f,0.75f,levelName);
    }
    public void OnBackgroundVolumeChange(Single value)
    {
        SoundManager.Instance.SetBackgroundVolume(value);
    }
    public void OnEffectsVolumeChange(Single value)
    {
        SoundManager.Instance.SetEffectsVolume(value);
    }
    private void DistortionDecrease(float value, float duration, string levelName)
    {
        DOTween.To(() => distortion.scale.value, x => distortion.scale.value = x, value, duration * 2);
        DOTween.To(() => distortion.intensity.value, x => distortion.intensity.value = x, value, duration).OnComplete(() => SceneManager.LoadScene(levelName));
    }
    private void OnDisable()
    {
        distortion.intensity.value = -0.5f;
        distortion.xMultiplier.value = 0;
        distortion.yMultiplier.value = 0;
        distortion.scale.value = 1;
    }
}
