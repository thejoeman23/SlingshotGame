using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage = 1;
    [HideInInspector] public GameObject target;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = (transform.position - target.transform.position).normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.GetComponent<Health>())
        {
            hit.GetComponent<Health>().takeDamage(damage, .5f);
            Destroy(gameObject);
        }
    }
}
