using System.Collections;
using System.IO;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f, backwardSpeed = 3f;
    [SerializeField] private GameObject effect  = null;
    private LineRenderer line = null;
    private int maxWayPoint,currentWaypoint;
    private Vector3[] positions;
    

    public void SetLine(LineRenderer line)
    {
        this.line = line;
        maxWayPoint = line.positionCount;
        positions = new Vector3[maxWayPoint];
        line.GetPositions(positions);
        StartCoroutine(MoveForward(0));

    }
    public void ChangeState(int direction)
    {
        StopAllCoroutines();
        if(direction == 1)
        {
            StartCoroutine(MoveForward(currentWaypoint + 1));
        }
        else if(direction == -1)
        {
            StartCoroutine(MoveBackward(currentWaypoint - 1));
        }
    }
    IEnumerator MoveForward(int startIndex)
    {
        for(int i = startIndex; i<maxWayPoint;i++)
        {
            currentWaypoint = i;
            while (Vector3.Distance(transform.position, positions[currentWaypoint]) > .0001)
            {
                transform.position = Vector3.MoveTowards(transform.position, positions[currentWaypoint], forwardSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = positions[currentWaypoint];
        }
    }
    IEnumerator MoveBackward(int startIndex)
    {
        for (int i = startIndex; i >= 0; i--)
        {
            currentWaypoint = i;
            while (Vector3.Distance(transform.position, positions[currentWaypoint]) > .0001)
            {
                transform.position = Vector3.MoveTowards(transform.position, positions[currentWaypoint], backwardSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = positions[currentWaypoint];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            GameManager.Instance.IncreaseCount(this);
        }
    }
}
