using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] int scene;
    [SerializeField] GameObject fadeObject;

    // Start is called before the first frame update
    void Start()
    {
        Button myButton = GetComponent<Button>();

        myButton.onClick.AddListener(sendToGame);
    }

    void sendToGame()
    {
        StartCoroutine(transition());
    }

    IEnumerator transition()
    {
        fadeObject.SetActive(true);
        fadeObject.GetComponent<Image>().DOFade(1, 1).SetLoops(1).Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }
}
