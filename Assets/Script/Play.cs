using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] int scene;

    // Start is called before the first frame update
    void Start()
    {
        Button myButton = GetComponent<Button>();

        myButton.onClick.AddListener(sendToGame);
    }

    void sendToGame()
    {
        SceneManager.LoadScene(scene);

    }
}
