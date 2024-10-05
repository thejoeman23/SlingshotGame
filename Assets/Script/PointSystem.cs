using DG.Tweening;
using System;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI player1Points;
    [SerializeField] TextMeshProUGUI player2Points;

    bool isFading1;
    bool isFading2;

    public void addPoints(bool isPlayer1, float points)
    {
        if (isPlayer1)
        {
            float currentPoints = int.Parse(player1Points.text);
            player1Points.text = (currentPoints + points).ToString();

            StartCoroutine(pointsEffect1());
        }
        else
        {
            float currentPoints = int.Parse(player2Points.text);
            player2Points.text = (currentPoints + points).ToString();

            StartCoroutine(pointsEffect2());
        }
    }

    IEnumerator pointsEffect1()
    {
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
    }

    IEnumerator pointsEffect2()
    {
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
    }
}
