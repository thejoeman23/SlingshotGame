using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoolScript : MonoBehaviour
{
    [SerializeField] Image logo;
    [SerializeField] Image fadeScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeScreen.color = Color.black;
        logo.color = Color.clear;

        StartCoroutine(beginingSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator beginingSequence()
    {
        logo.DOColor(Color.white, 1).SetLoops(1).Play();
        yield return new WaitForSeconds(2f);
        logo.DOFade(0, 1).SetLoops(1).Play();
        yield return new WaitForSeconds(2f);
        fadeScreen.DOFade(0, 1).SetLoops(1).Play();
    }
}
