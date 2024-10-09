using DG.Tweening;
using System;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    // the text that displays the players points
    [SerializeField] GameObject player1Health;
    [SerializeField] GameObject player2Health;

    [SerializeField] GameObject winScreen;
    [SerializeField] TextMeshProUGUI winTextObject;

    [SerializeField] GameObject winParticles;

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [SerializeField] bool singlePlayer;


    // current points of the player
    float player1Current = 5;
    float player2Current = 5;

    bool isFading1;
    bool isFading2;

    private void Start()
    {
        winScreen.SetActive(false);
    }

    public void addPoints(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player1Current--;

            if (player1Current <= 0)
            {
                endGame(player2, " Wins!!");
                StartCoroutine(healthEffect(player1Health, player1Current));

            } else { StartCoroutine(healthEffect(player1Health, player1Current)); }
        }
        else
        {
            player2Current--;

            if (player2Current <= 0)
            {
                endGame(player1, " Wins!!");
                StartCoroutine(healthEffect(player2Health, player2Current));

            } else { StartCoroutine(healthEffect(player2Health, player2Current)); }
        }
    }

    IEnumerator healthEffect(GameObject playerHealth, float current)
    {
        for (float i = current + 1; i > 0; i--)
        {
            GameObject child = playerHealth.transform.GetChild((int)i - 1).gameObject;
            
            // if its the heart closest to the end then destroy it
            if (i == playerHealth.transform.childCount) { Destroy(child); continue; }

            // shake the other hearts
            child.transform.DOLocalMoveY(-8, .1f).SetLoops(1).Play();
            yield return new WaitForSeconds(.1f);
            child.transform.DOLocalMoveY(-14, .1f).SetLoops(1).Play();
        }
    }

    void endGame(GameObject winner, string message)
    {
        Instantiate(winParticles, winner.transform.position, Quaternion.identity);

        winScreen.SetActive(true);
        winScreen.transform.DOLocalMove(Vector3.zero, 1).SetLoops(1).Play();

        if (singlePlayer) winTextObject.text = "Play Again?";
        else winTextObject.text = winner.name + message;
    }
}
