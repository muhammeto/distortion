using Chronos.Example;
using System.Collections;
using System.IO;
using UnityEngine;

public class CircleMove : ExampleBaseBehaviour
{
    [SerializeField] private float forwardSpeed = 5f, backwardSpeed = 3f;
    private int _maxWayPoint,_currentWaypoint;
    private Vector3[] _positions;
    
    public void SetLine(Vector3[] positions)
    {
        _positions = positions;
        _maxWayPoint = _positions.Length;
        _currentWaypoint = 0;
        StartCoroutine(MoveForward(_currentWaypoint));
    }
    public void ChangeState(GameState state)
    {
        StopAllCoroutines();
        switch (state)
        {
            case GameState.Forward:
                StartCoroutine(MoveForward(_currentWaypoint + 1));
                break;
            case GameState.Backward:
                StartCoroutine(MoveBackward(_currentWaypoint - 1));
                break;
        }
    }
    IEnumerator MoveForward(int startIndex)
    {
        for(int i = startIndex; i<_maxWayPoint;i++)
        {
            _currentWaypoint = i;
            while (Vector3.Distance(transform.position, _positions[_currentWaypoint]) > .0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _positions[_currentWaypoint], forwardSpeed * time.deltaTime);
                yield return null;
            }
            transform.position = _positions[_currentWaypoint];
        }
    }
    IEnumerator MoveBackward(int startIndex)
    {
        for (int i = startIndex; i >= 0; i--)
        {
            _currentWaypoint = i;
            while (Vector3.Distance(transform.position, _positions[_currentWaypoint]) > .0001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _positions[_currentWaypoint], backwardSpeed * time.deltaTime);
                yield return null;
            }
            transform.position = _positions[_currentWaypoint];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            GameManager.Instance.IncreaseCount(this);
        }else if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance.LosePanel(transform.position);
        }
    }
}
