using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private VolumeProfile postProcess = null;
    [SerializeField] private GameObject rewindText = null;
    [SerializeField] private GameObject losePanel = null, winPanel = null;
    [SerializeField] private int neededCircles = 0;
    [SerializeField] private LineRenderer line = null;
    [SerializeField] private GameObject circle = null;
    [SerializeField] private Vector2 offsetCircles = Vector2.zero;

    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    private UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    private int currentCircles = 0;
    private bool isForward = true;
    private List<CircleMove> circles;
    private Camera cam;
    private bool died= false;

    private void Start()
    {
        circles = new List<CircleMove>();
        for (int i = 0; i < neededCircles; i++)
        {
            Vector2 pos = line.GetPosition(0);
            pos += offsetCircles*i;
            CircleMove current = Instantiate(circle, pos, Quaternion.identity).GetComponent<CircleMove>();
            current.SetLine(line);
            circles.Add(current);
        }

        postProcess.TryGet(out chromatic);
        postProcess.TryGet(out distortion);
        chromatic.intensity.value = 0;
        distortion.intensity.value = 0;
        
        cam = Camera.main;
    }
    private void Update()
    {
        if (died) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState();
        }
    }
    private void ChangeState()
    {
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].ChangeState(isForward ? -1 : 1);
        }
        StopAllCoroutines();
        SoundManager.Instance.ChangeState();
        if (isForward)
        {
            rewindText.SetActive(true);
            isForward = false;
            StartCoroutine(ChromaticIncrease(1f));
            StartCoroutine(DistortionDecrease(-0.6f));
        }
        else
        {
            rewindText.SetActive(false);
            isForward = true;
            StartCoroutine(ChromaticDecrease(0));
            StartCoroutine(DistortionIncrease(0));

        }
    }
    private void Retry()
    {
        SoundManager.Instance.Click();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void NextLevel()
    {
        SoundManager.Instance.Click();
        // Change this
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
        cam.transform.DOMove(zoomPos, 0.5f).OnComplete(()=>
        {
            losePanel.SetActive(true);
            losePanel.transform.DOLocalMoveX(0, 0.25f);
        });
    }
    public void IncreaseCount(CircleMove cmove)
    {
        currentCircles++;
        circles.Remove(cmove);
        Destroy(cmove.gameObject);
        if (currentCircles == neededCircles)
        {
            StopGame();
            winPanel.SetActive(true);
            winPanel.transform.DOLocalMoveX(0, 0.5f);
        }
    }
    private void StopGame()
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
    private IEnumerator ChromaticIncrease(float value)
    {
        while (value - chromatic.intensity.value > 0.01)
        {
            chromatic.intensity.value += 0.005f;
            yield return null;
        }
    }
    private IEnumerator ChromaticDecrease(float value)
    {
        while (chromatic.intensity.value - value > 0.01)
        {
            chromatic.intensity.value -= 0.005f;
            yield return null;
        }
    }
    private IEnumerator DistortionIncrease(float value)
    {
        while (value - distortion.intensity.value > 0.01)
        {
            distortion.intensity.value += 0.005f;
            yield return null;
        }
    }
    private IEnumerator DistortionDecrease(float value)
    {
        while (distortion.intensity.value - value > 0.01)
        {
            distortion.intensity.value -= 0.005f;
            yield return null;
        }
    }
}
