using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CircleEffect : MonoBehaviour
{
    [SerializeField] GameObject _circle;
    [SerializeField] float effectSpeed;

    public void playEffect(GameObject Object)
    {
        StartCoroutine(circleEffect(Object));
    }

    IEnumerator circleEffect(GameObject effectObject)
    {
        GameObject circle = Instantiate(_circle, effectObject.transform.position, Quaternion.identity, effectObject.transform);
        SpriteRenderer sr = circle.GetComponent<SpriteRenderer>();
        circle.transform.DOScale(new Vector3(.75f, .75f, 0), effectSpeed).Play();

        sr.DOColor(Color.clear, effectSpeed * 2.5f).Play();

        Debug.Log("skds");
        yield return new WaitForSeconds(effectSpeed);

        Destroy(circle);

        yield return null;
    }
}
