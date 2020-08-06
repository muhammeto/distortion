using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MenuUI: MonoBehaviour
{
    [SerializeField] private RectTransform startButton=null, modePanel=null;
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
        startButton.DOLocalMoveX(-1500, 0.5f);
        modePanel.DOLocalMoveX(0, 0.5f);
    }

    public void OnLevelModeButtonClick()
    {
        distortion.xMultiplier.value = 1;
        distortion.yMultiplier.value = 1;
        StartCoroutine(DistortionDecrease(-1f, "Level1"));

    }

    public void OnEndlessModeButtonClick()
    {
        distortion.xMultiplier.value = 1;
        distortion.yMultiplier.value = 1;
        StartCoroutine(DistortionDecrease(-1f,"Endless"));
    }
}
