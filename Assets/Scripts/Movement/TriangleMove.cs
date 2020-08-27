using Chronos.Example;
using UnityEngine;

public class TriangleMove : ExampleBaseBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Vector2 direction = Vector2.right;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = time.GetComponent<Rigidbody2D>();
        _rb.velocity = direction*speed;
    }
}
