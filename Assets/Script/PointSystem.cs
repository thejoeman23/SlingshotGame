using DG.Tweening;
using System;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    // the text that displays the players points
    [SerializeField] TextMeshProUGUI player1Points;
    [SerializeField] TextMeshProUGUI player2Points;

    [SerializeField] GameObject winScreen;
    [SerializeField] TextMeshProUGUI winTextObject;

    [SerializeField] GameObject winParticles;

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    // current points of the player
    float player1Current;
    float player2Current;

    bool isFading1;
    bool isFading2;

    private void Start()
    {
        winScreen.SetActive(false);
    }

    private void Update()
    {
        player1Points.text = player1Current.ToString();
        player2Points.text = player2Current.ToString();
    }

    public void addPoints(bool isPlayer1, float points)
    {
        if (isPlayer1)
        {
            player1Current += points;

            StartCoroutine(pointsEffect1());

            if (player1Current >= 10)
            {
                endGame(player2);
            }
        }
        else
        {
            player2Current += points;

            StartCoroutine(pointsEffect2());

            if (player2Current >= 10)
            {
                endGame(player1);
            }
        }
    }

    IEnumerator pointsEffect1()
    {
        #region
        if (!isFading1)
        {
            isFading1 = true;

            Tween clearText = player1Points.DOColor(Color.clear, .5f).SetLoops(1);
            clearText.Play();

            yield return new WaitForSeconds(.5f);

            Tween normalText = player1Points.DOColor(Color.white, .5f).SetLoops(1);
            normalText.Play();

            isFading1 = false;

            yield return null;
        } else
        {
           yield return null;
        }
        #endregion
    }

    IEnumerator pointsEffect2()
    {
        #region
        if (!isFading2)
        {
            isFading2 = true;

            Tween clearText = player2Points.DOColor(Color.clear, .5f).SetLoops(1);
            clearText.Play();

            yield return new WaitForSeconds(.5f);

            Tween normalText = player2Points.DOColor(Color.white, .5f).SetLoops(1);
            normalText.Play();

            isFading2 = false;

            yield return null;
        }
        else
        {
            yield return null;
        }
        #endregion
    }

    void endGame(GameObject winner)
    {
        Instantiate(winParticles, winner.transform.position, Quaternion.identity);

        winScreen.SetActive(true);
        winScreen.transform.DOLocalMove(Vector3.zero, 1).SetLoops(1).Play();

        winTextObject.text = winner.name + " Wins!!";
    }
}
