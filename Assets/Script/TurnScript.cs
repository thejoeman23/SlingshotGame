using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] TextMeshProUGUI turnText;

    bool player1Turn = true;

    private void Start()
    {
        player1.SetActive(true);
        player2.SetActive(false);

        turnText.text = "Player 1";
    }

    public void switchTurns()
    {
        player1Turn = !player1Turn;

        if (player1Turn)
        {
            player1.SetActive(true);
            player2.SetActive(false);

            StartCoroutine(fadeInOut());

            turnText.text = "Player 1";
        } else
        {
            player1.SetActive(false);
            player2.SetActive(true);

            StartCoroutine(fadeInOut());

            turnText.text = "Player 2";
        }
    }

    IEnumerator fadeInOut()
    {
        Tween clearText = turnText.DOColor(Color.clear, .5f).SetLoops(1);
        clearText.Play();

        yield return new WaitForSeconds(.5f);

        Tween normalText = turnText.DOColor(Color.white, .5f).SetLoops(1);
        normalText.Play();

        yield return null;
    }
}
