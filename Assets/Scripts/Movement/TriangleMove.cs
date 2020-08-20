using Chronos.Example;
using UnityEngine;

public class TriangleMove : ExampleBaseBehaviour
{
    private Rigidbody2D _rb;
    private bool isVisible;
    [SerializeField] private GameObject indicator;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private LayerMask camBounds;
    private void Start()
    {
        _rb = time.GetComponent<Rigidbody2D>();
        _rb.velocity = direction*speed;
    }

    private void Update()
    {
        if (!isVisible)
        {
            if (!indicator.activeSelf) indicator.SetActive(true);
            Vector2 direction = -transform.position.normalized;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction,10, camBounds);
            if (ray.collider != null)
            {
                indicator.transform.position = ray.point;
            }
        }
        else
        {
            if (indicator.activeSelf) indicator.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    public void SetSpeed(float speed = 0f)
    {
        _rb.velocity = direction*speed;
    }
}
