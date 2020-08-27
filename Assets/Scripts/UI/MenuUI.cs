using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI: MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel=null, levelPanel=null, optionsPanel=null,quitPanel=null;
    [SerializeField] private GameObject menuFirstSelected = null, levelFirstSelected = null, optionsFirstSelected = null, quitFirstSeleted = null;
    [SerializeField] private VolumeProfile postProcess = null;
    [SerializeField] private Slider backgroundVolume = null, effectsVolume = null;
    [SerializeField] private Toggle inputToggle = null;
    private LensDistortion distortion;

    private void Start()
    {
        postProcess.TryGet(out distortion);
        distortion.intensity.value = -0.5f;
        distortion.xMultiplier.value = 0;
        distortion.yMultiplier.value = 0;
        distortion.scale.value = 1;
        backgroundVolume.value = PlayerPrefs.GetFloat("BackgroundVolume", 0.5f);
        effectsVolume.value = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
        inputToggle.isOn = PlayerPrefs.GetInt("InputType", 0) == 0 ? false : true;
    }

    public void ChangeSelected(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    
    public void OnPlayButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();
        menuPanel.DOLocalMoveX(-2000, 0.5f);
        levelPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelected(levelFirstSelected);
    }
    public void OnPlayBackButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();

        levelPanel.DOLocalMoveX(2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelected(menuFirstSelected);

    }
    public void OnOptionsButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();

        menuPanel.DOLocalMoveX(-2000, 0.5f);
        optionsPanel.DOLocalMoveY(0, 0.5f);
        ChangeSelected(optionsFirstSelected);

    }
    public void OnOptionsBackButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();

        optionsPanel.DOLocalMoveY(2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelected(menuFirstSelected);
    }
    public void OnQuitButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();

        menuPanel.DOLocalMoveX(-2000, 0.5f);
        quitPanel.DOLocalMoveY(0, 0.5f);
        ChangeSelected(quitFirstSeleted);
    }
    public void OnQuitBackButtonClick()
    {
        SoundManager.Instance.Click();
        SoundManager.Instance.Slide();

        quitPanel.DOLocalMoveY(-2000, 0.5f);
        menuPanel.DOLocalMoveX(-135, 0.5f);
        ChangeSelected(menuFirstSelected);
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
    public void OnToggleValueChange(bool change)
    {
        PlayerPrefs.SetInt("InputType", change?1:0);
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
