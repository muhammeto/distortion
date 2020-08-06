using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMove : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float minSpeed = 3f, maxSpeed = 6f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(Random.Range(minSpeed, maxSpeed), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.LosePanel(transform.position);
        }else if (collision.CompareTag("DestroyMe"))
        {
            Destroy(gameObject);
        }
    }

    public void Stop()
    {
        _rb.velocity = Vector2.zero;
    }
}
