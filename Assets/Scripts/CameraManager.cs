using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : Singleton<CameraManager>
{
    private Camera _currentCamera;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentCamera = Camera.main;
    }
    public void ShakePosition(float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
    {
        _currentCamera.DOShakePosition(duration,strength,vibrato,randomness,fadeOut);
    }
    public void Zoom(Vector3 zoomPos,float duration = 0.5f,float orhtoSize = 2)
    {
        zoomPos = new Vector3(zoomPos.x, zoomPos.y, -10);
        _currentCamera.DOOrthoSize(orhtoSize, duration);
        _currentCamera.transform.DOMove(zoomPos, duration);
    }
}
