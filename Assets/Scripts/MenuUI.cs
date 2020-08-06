using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuUI: MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel=null, levelPanel=null;
    [SerializeField] private VolumeProfile postProcess = null;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    private void Start()
    {
        postProcess.TryGet(out distortion);
        distortion.intensity.value = -0.5f;
        distortion.xMultiplier.value = 0;
        distortion.yMultiplier.value = 0;
        distortion.scale.value = 1;
    }

    IEnumerator DistortionDecrease(float value,string levelName)
    {
        while (distortion.intensity.value - value > 0.01)
        {
            distortion.intensity.value -= 0.0025f;
            distortion.scale.value -= 0.005f;
            yield return null;
        }
        SceneManager.LoadScene(levelName);
    }

    public void OnPlayButtonClick()
    {
        Camera.main.DOShakePosition(0.1f);
        menuPanel.DOLocalMoveX(-1500, 0.5f);
        levelPanel.DOLocalMoveX(0, 0.5f);
        SoundManager.Instance.Click();
    }

    public void OnLevelButtonClick(string levelName)
    {
        distortion.xMultiplier.value = 1;
        distortion.yMultiplier.value = 1;
        SoundManager.Instance.Click();
        StartCoroutine(DistortionDecrease(-1f,levelName));

    }
  
    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
