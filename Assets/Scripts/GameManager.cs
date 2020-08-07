using DG.Tweening;
using DG.Tweening.Core;
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
    [SerializeField] private GameObject losePanel = null, winPanel = null,pausePanel = null;
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
    private bool isPaused=false;

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
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pausePanel.transform.DOLocalMoveY(0, 0.25f).OnComplete(()=>Time.timeScale = 0);
            }
            else
            {
                pausePanel.transform.DOLocalMoveY(900, 0.25f).OnComplete(() => Time.timeScale = 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began))
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
        DOTween.KillAll();
        SoundManager.Instance.ChangeState();
        if (isForward)
        {
            rewindText.SetActive(true);
            isForward = false;
            ChromaticChange(1f,0.75f);
            DistortinChange(-0.5f, 0.75f);
        }
        else
        {
            rewindText.SetActive(false);
            isForward = true;
            ChromaticChange(0f, 0.75f);
            DistortinChange(0f, 0.75f);
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
            if(SceneManager.GetActiveScene().name == "Level10")
            {
                SceneManager.LoadScene("Menu");
            }
            winPanel.transform.DOLocalMoveX(0, 0.5f);
        }
    }
    public void OnMenuButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    public void OnContinueButtonClick()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.transform.DOLocalMoveY(900, 0.25f);
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
    private void ChromaticChange(float value,float duration)
    {
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, value, duration);
    }
    private void DistortinChange(float value, float duration)
    {
        DOTween.To(() => distortion.intensity.value, x => distortion.intensity.value = x, value, duration);
    }
    
}
