using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    [SerializeField] private VolumeProfile postProcess = null;
    [SerializeField] private GameObject rewindText = null;
    [SerializeField] private GameObject losePanel,winPanel = null;
    [SerializeField] private int neededCircles = 0;
    private int currentCircles = 0;
    private AudioSource rewindSource;
    private bool isForward = true;
    private List<CircleMove> circles;
    private Camera cam;
    private bool died=false;
    void Start()
    {
        circles = FindObjectsOfType<CircleMove>().ToList();
        rewindSource = GetComponent<AudioSource>();
        postProcess.TryGet(out chromatic);
        postProcess.TryGet(out distortion);
        chromatic.intensity.value = 0;
        distortion.intensity.value = 0;
        cam = Camera.main;
    }
    void Update()
    {
        if (died) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < circles.Count; i++)
            {
                circles[i].ChangeState(isForward?-1:1);
            }
            StopAllCoroutines();
            SoundManager.Instance.ChangeState();
            if (isForward)
            {
                rewindText.SetActive(true);
                isForward = false;
                rewindSource.Play();
                StartCoroutine(ChromaticIncrease(1f));
                StartCoroutine(DistortionDecrease(-0.6f));
            }
            else
            {
                rewindText.SetActive(false);
                isForward = true;
                rewindSource.Stop();

                StartCoroutine(ChromaticDecrease(0));
                StartCoroutine(DistortionIncrease(0));

            }
        }
    }
    public void IncreaseCount(CircleMove cmove)
    {
        currentCircles++;
        circles.Remove(cmove);
        Destroy(cmove.gameObject);
        if(currentCircles == neededCircles)
        {
            winPanel.SetActive(true);
            winPanel.transform.DOLocalMoveX(0, 0.5f);
        }
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < 8)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void LosePanel(Vector3 zoomPos)
    {
        StopGame();
        died = true;
        zoomPos = new Vector3(zoomPos.x, zoomPos.y, -10);
        cam.DOOrthoSize(2, 0.5f);
        cam.DOShakePosition(0.5f,8);
        cam.transform.DOMove(zoomPos, 0.5f).OnComplete(()=>
        {
            losePanel.SetActive(true);
            losePanel.transform.DOLocalMoveX(0, 0.25f);
        });
    }
    public void StopGame()
    {
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].ChangeState(0);
        }
        TriangleMove[] triangles = FindObjectsOfType<TriangleMove>();
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i].Stop();
        }
    }
    IEnumerator ChromaticIncrease(float value)
    {
        while (value - chromatic.intensity.value > 0.01)
        {
            chromatic.intensity.value += 0.005f;
            yield return null;
        }
    }
    IEnumerator ChromaticDecrease(float value)
    {
        while (chromatic.intensity.value - value > 0.01)
        {
            chromatic.intensity.value -= 0.005f;
            yield return null;
        }
    }
    IEnumerator DistortionIncrease(float value)
    {
        while (value - distortion.intensity.value > 0.01)
        {
            distortion.intensity.value += 0.005f;
            yield return null;
        }
    }
    IEnumerator DistortionDecrease(float value)
    {
        while (distortion.intensity.value - value > 0.01)
        {
            distortion.intensity.value -= 0.005f;
            yield return null;
        }
    }
}
