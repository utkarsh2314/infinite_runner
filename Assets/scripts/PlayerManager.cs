using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted ;
    public GameObject startingText;

    public static int coins;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI coinText1;
    public TextMeshProUGUI coinText2;

    // Start is called before the first frame update
    void Start()
    {
        isGameStarted = false;
        gameOver = false;
        coins=0;
}

    // Update is called once per frame
    void Update()
    {
        coinText.text = "COIN: " + coins;
        coinText1.text = "COIN: " + coins;
        coinText2.text = "COIN: " + coins;
        if (PlayerMove.tap && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }

        if (gameOver)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }
    }
}
