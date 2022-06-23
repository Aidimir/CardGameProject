using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UIManager : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;
    public GameObject Button;
    public GameObject PlayerText;
    public GameObject OpponentText;
    public GameObject WonImage;
    public GameObject LostImage;
    public GameObject ScoreText;
    public GameObject Canvas;

    Color blueColor = new Color32(17, 216, 238, 255);

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();    
        Canvas = GameObject.Find("Main Canvas");

    }

    public void UpdatePlayerText()
    {
        PlayerText.GetComponent<Text>().text = "ваши очки: " + GameManager.PlayerBP;
        OpponentText.GetComponent<Text>().text = "вражеские очки: " + GameManager.OpponentBP;
    }

    public void UpdateScoreText()
    {
        ScoreText.GetComponent<Text>().text = "Общий счет " + GameManager.PlayerWins + ":" + GameManager.OpponentWins;
    }

    public void UpdateButtonText(string gameState)
    {
        Button = GameObject.Find("Button");
        Button.GetComponentInChildren<Text>().text = gameState;
    }

    public void HighlightTurn (int turnOrder)
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        if (turnOrder < 10)
        {
            if (turnOrder == 0)
            {
                ResetHighlight();
                PlayerManager.CardsPlayed = 0;
                if (PlayerManager.IsMyTurn)
                {
                    PlayerManager.PlayerSockets[PlayerManager.CardsPlayed].GetComponent<Outline>().effectColor = Color.magenta;
                }
                else
                {
                    PlayerManager.EnemySockets[4-PlayerManager.CardsPlayed].GetComponent<Outline>().effectColor = Color.magenta;
                }
            }
            else if (turnOrder > 0)
            {
                if (PlayerManager.IsMyTurn)
                {
                    PlayerManager.PlayerSockets[PlayerManager.CardsPlayed].GetComponent<Outline>().effectColor = Color.magenta;

                    if (isClientOnly && turnOrder > 1)
                    {
                        PlayerManager.EnemySockets[PlayerManager.CardsPlayed - 1 + 4].GetComponent<Outline>().effectColor = blueColor;
                    }
                    else
                    {
                        PlayerManager.EnemySockets[PlayerManager.CardsPlayed + 4].GetComponent<Outline>().effectColor = blueColor;
                    }
                }
                else
                {
                    PlayerManager.PlayerSockets[PlayerManager.CardsPlayed - 1].GetComponent<Outline>().effectColor = blueColor;

                    if (isClientOnly)
                    {
                        PlayerManager.EnemySockets[PlayerManager.CardsPlayed - 1 + 4].GetComponent<Outline>().effectColor = Color.magenta;
                    }
                    else
                    {
                        PlayerManager.EnemySockets[PlayerManager.CardsPlayed + 4].GetComponent<Outline>().effectColor = Color.magenta;
                    }
                }
            }
        }
        else if (turnOrder == 10)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerManager.PlayerSockets[i].GetComponent<Outline>().effectColor = blueColor;
                PlayerManager.EnemySockets[i].GetComponent<Outline>().effectColor = blueColor;
            }
        }
    }

    public void ResetHighlight()
    {
        for (int i = 0; i < PlayerManager.PlayerSockets.Count; i++) 
        {
            PlayerManager.PlayerSockets[i].GetComponent<Outline>().effectColor = blueColor;
            PlayerManager.EnemySockets[i].GetComponent<Outline>().effectColor = blueColor; 
        }
    }

    public void SetImage(string result)
    {
        if (result == "won")
        {
            GameObject img= Instantiate(WonImage, new Vector2(0, 0), Quaternion.identity);
            img.transform.SetParent(Canvas.transform, false);
        }
        else
        {
            GameObject img= Instantiate(LostImage, new Vector2(0, 0), Quaternion.identity);
            img.transform.SetParent(Canvas.transform, false);
        }
    }
}
