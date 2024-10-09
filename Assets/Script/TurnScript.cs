using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] string p1Text = "Player 1";
    [SerializeField] string p2Text = "Player 2";

    bool player1Turn = true;

    private void Start()
    {
        player1.SetActive(true);
        player2.SetActive(false);

        turnText.text = p1Text;
    }

    public void switchTurns()
    {
        player1Turn = !player1Turn;

        if (player1Turn) activateRight(); else activateLeft();
        StartCoroutine(fadeInOut());
        turnText.text = player1Turn ? p1Text : p2Text;
    }

    public void activateLeft() {
            player1.SetActive(false);
            player2.SetActive(true);
    }

    public void activateRight() {
            player1.SetActive(true);
            player2.SetActive(false);
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
