using UnityEngine;

public class wasdPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight;

    [SerializeField] float minJumpDistance;

    Rigidbody2D rb;
    [SerializeField] bool grounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(input * moveSpeed, 0);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            grounded = false;
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpHeight));
        }

        Vector2 offset = new Vector2(0, (transform.localScale.y / 2) + .05f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position - offset, Vector2.down, (int)minJumpDistance);
        if (hit)
        {
            grounded = true;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 offset = new Vector2(0, (transform.localScale.y / 2) + .05f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector2)transform.position - offset, (Vector2)transform.position - new Vector2(0, minJumpDistance));
    }
}
