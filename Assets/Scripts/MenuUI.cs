using DG.Tweening;
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

    void DistortionDecrease(float value,float duration,string levelName)
    {
        DOTween.To(() => distortion.scale.value, x => distortion.scale.value = x, value, duration*2);
        DOTween.To(() => distortion.intensity.value, x => distortion.intensity.value = x, value, duration).OnComplete(()=> SceneManager.LoadScene(levelName));
    }

    public void OnPlayButtonClick()
    {
        print("clicked");
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
        DistortionDecrease(-1f,0.75f,levelName);

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
