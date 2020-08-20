using Chronos;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Forward,
    Backward,
    Pause
}
public class GameManager : UnitySingleton<GameManager>
{
    [SerializeField] private int neededCircles = 0;
    [SerializeField] private LineRenderer line = null;
    [SerializeField] private GameObject circle = null;
    [SerializeField] private Vector2 offsetCircles = Vector2.zero;
    [SerializeField] private GameObject effect = null;
    

    private GameState _state;
    private int currentCircles = 0;
    private bool died = false, paused = false, finished = false;
    private List<CircleMove> circles;
    private Vector3[] _positions;

    private PostprocessManager postprocessManager;
    private GameUIManager UIManager;
    private Camera cam;

    private void Start()
    {
        postprocessManager = GetComponent<PostprocessManager>();
        UIManager = GetComponent<GameUIManager>();
        cam = Camera.main;
        _state = GameState.Forward;
        circles = new List<CircleMove>();
        _positions = new Vector3[line.positionCount];
        line.GetPositions(_positions);
        for (int i = 0; i < neededCircles; i++)
        {
            Vector2 pos = line.GetPosition(0);
            pos += offsetCircles*i;
            CircleMove current = Instantiate(circle, pos, Quaternion.identity).GetComponent<CircleMove>();
            current.SetLine(_positions);
            circles.Add(current);
        }
    }
    private void Update()
    {
        if (died || finished) return;
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!paused)
            {
                UIManager.OnPause();
            }
            else
            {
                UIManager.OnContinueButtonClick();
            }
            paused = !paused;
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space) || (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            ChangeState(_state == 0 ? GameState.Backward : GameState.Forward);
        }
    }
    private void ChangeState(GameState state)
    {
        _state = state;
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].ChangeState(_state);
        }
        postprocessManager.ChangeState(_state);
        SoundManager.Instance.ChangeState(_state);
        UIManager.ChangeState(_state == GameState.Backward);
    }
    
    public void LosePanel(Vector3 zoomPos)
    {
        died = true;
        CameraManager.Instance.Zoom(zoomPos);
        UIManager.OnLose();
    }
    public void IncreaseCount(CircleMove cmove)
    {
        currentCircles++;
        Instantiate(effect, cmove.transform.position, Quaternion.identity);
        circles.Remove(cmove);
        Destroy(cmove.gameObject);
        if (currentCircles == neededCircles)
        {
            finished = true;
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount-1)
            {
                SceneManager.LoadScene("Menu");
            }
            UIManager.OnWin();
        }
    }
    private void StopGame()
    {
        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].ChangeState(GameState.Pause);
        }
        TriangleMove[] triangles = FindObjectsOfType<TriangleMove>();
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i].SetSpeed();
        }
    }
    
}
