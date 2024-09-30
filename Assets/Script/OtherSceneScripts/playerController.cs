using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour
{
    bool isAttacking;
    bool canMove = true;

    [SerializeField] float speed;
    [SerializeField] float attackSpeed;
    [SerializeField] GameObject attackHitbox;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            rb.linearVelocity = new Vector2(horizontal, vertical).normalized * speed;
        } else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (!isAttacking && Input.GetMouseButtonDown(0))
        {
            Debug.Log("attacked");
            StartCoroutine(attack());
        }
    }

    IEnumerator attack()
    {
        canMove = false;
        isAttacking = true;
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackSpeed);

        canMove = true;
        isAttacking = false;
        attackHitbox.SetActive(false);
    }
}
