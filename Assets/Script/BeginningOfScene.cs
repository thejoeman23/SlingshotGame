using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeginningOfScene : MonoBehaviour
{
    [SerializeField] Image fadeScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeScreen.gameObject.SetActive(true);

        StartCoroutine(beginingSequence());
    }

    IEnumerator beginingSequence()
    {
        fadeScreen.DOFade(0, 1).SetLoops(1).Play();
        yield return null;
    }
}
