using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using  UnityEngine.SceneManagement;
public class GameManager : NetworkBehaviour
{
    public UIManager UIManager;
    public GameObject Canvas;
    public PlayerManager PlayerManager;
    public int TurnOrder = 0;
    public string GameState = "Initialize {}";
    public int PlayerBP = 0;
    public int OpponentBP = 0;
    public int PlayerVariables = 0;
    public int OpponentVariables = 0;
	public int PlayerWins = 0;
	public int OpponentWins = 0;
    private int ReadyClicks = 0;

    void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        UIManager.UpdatePlayerText();
        UIManager.UpdateButtonText(GameState);
        Canvas = GameObject.Find("Main Canvas");
    }
    
    public void ChangeGameState(string stateRequest)
    {
        if (stateRequest == "Initialize {}")
        {
            ReadyClicks = 0;
            GameState = "Initialize {}";
        }
        else if (stateRequest == "Compile {}")
        {
            if (ReadyClicks == 1)
            {
                GameState = "Compile {}";
                UIManager.HighlightTurn(TurnOrder);
                Debug.Log("READY CLICKS == 1 MESSAGE");
            }
        }
        else if (stateRequest == "Execute {}")
        {
            GameState = "Execute {}";
            TurnOrder = 0;
            PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            ChangeScore(false,false);
            Debug.Log("EXECUTE MESSAGE");

        }
        else if ((stateRequest == "Surrender0") || stateRequest == "Surrender1")
        {
            GameState = "Surrender";
            TurnOrder = 0;
            PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            // PlayerManager.ResetCardsPlayed();
            UIManager.UpdateButtonText(GameState);
            if (stateRequest == "Surrender0")
            {
             ChangeScore(true,false);   
            }
            else
            {
                ChangeScore(false,true);
            }

        }
        UIManager.UpdateButtonText(GameState);
    }

    public void ChangeReadyClicks()
    {
        ReadyClicks++;
    }

    public void CardPlayed()
    {
        TurnOrder++;
        UIManager.HighlightTurn(TurnOrder);
        if (TurnOrder == 10)
        {
            ChangeGameState("Execute {}");
        }
    }

    public void ChangeBP (int playerBP, int opponentBP, bool hasAuthority)
    {
        if (hasAuthority)
        {
            PlayerBP += playerBP;
            OpponentBP -= opponentBP;
        }
        else
        {
            PlayerBP -= opponentBP;
            OpponentBP += playerBP;
        }
        UIManager.UpdatePlayerText();
    }

    public void ChangeVariables (int variables, bool hasAuthority)
    {
        if (hasAuthority)
        {
            PlayerVariables += variables;
        }
        else
        {
            OpponentVariables += variables;
        }
        UIManager.UpdatePlayerText();
    }

    public void ChangeScore(bool playerSurrender, bool enemySurrender )
    {
        RpcChangeScore(playerSurrender,enemySurrender);
    }

    // [Command]
    // public void CmdChangeScore()
    // {
    //     RpcChangeScore();
    // }

    [ClientRpc]
    public void RpcChangeScore(bool playerSurrender, bool enemySurrender)
    {
        if ((PlayerBP > OpponentBP) || (enemySurrender)) 
            {
                PlayerWins += 1;
            }
        else if (PlayerBP == OpponentBP && enemySurrender == false && playerSurrender == false)
            {
                PlayerWins += 1;
                OpponentWins += 1;
            }
        else
        {
            OpponentWins += 1;
        }

        PlayerBP = 0;
            OpponentBP = 0;
            UIManager.UpdatePlayerText();
            UIManager.UpdateScoreText();
            if ((PlayerWins == 2) || (OpponentWins == 2))
            {
                if ((PlayerWins == 2) && (OpponentWins == 2))
                {
                    print("НИЧЬЯ");
                }

                if ((PlayerWins == 2) && (OpponentWins != 2))
                {
                    UIManager.SetImage("won");
                }

                if ((PlayerWins != 2) && (OpponentWins == 2))
                {
                    UIManager.SetImage("lost");
                }
            }
    }
}
