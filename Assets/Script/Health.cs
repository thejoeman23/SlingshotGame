using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] bool stunnable = true;
    bool invinsible;

    public void takeDamage(float damage, float stunTime)
    {
        health -= damage;
        StartCoroutine(stun(stunTime));

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator stun(float stunTime)
    {
        invinsible = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        
        yield return new WaitForSeconds(stunTime);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        invinsible = false;
    }
}
