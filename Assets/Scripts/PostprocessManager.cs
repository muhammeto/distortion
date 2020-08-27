using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
public class PostprocessManager : MonoBehaviour
{
    [SerializeField] private VolumeProfile postProcess = null;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    private UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    private void Start()
    {
        postProcess.TryGet(out chromatic);
        postProcess.TryGet(out distortion);
        chromatic.intensity.value = 0;
        distortion.intensity.value = 0;
    }
    public void ChangeState(GameState state)
    {
        transform.DOKill();
        switch (state)
        {
            case GameState.Backward:
                ChromaticChange(1f, 0.75f);
                DistortionChange(-0.5f, 0.75f);
                break;
            case GameState.Forward:
                ChromaticChange(0f, 0.75f);
                DistortionChange(0f, 0.75f);
                break;
        }
    }

    public void ChromaticChange(float value, float duration)
    {
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, value, duration)
            .OnKill(() => chromatic.intensity.value = value);
    }
    public void DistortionChange(float value, float duration)
    {
        DOTween.To(() => distortion.intensity.value, x => distortion.intensity.value = x, value, duration)
            .OnKill(() => distortion.intensity.value = value);
    }

}
