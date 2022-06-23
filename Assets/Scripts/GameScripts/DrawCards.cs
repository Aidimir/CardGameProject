using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameManager GameManager;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        if (GameManager.GameState == "Initialize {}")
        {
            InitializeClick();
        }
        else if (GameManager.GameState == "Execute {}" || GameManager.GameState == "Surrender")
        {
            ExecuteClick();
        }
    }

    void InitializeClick()
    {
        PlayerManager.CmdDealCards();
        PlayerManager.CardsPlayed = 0;
    }

    void ExecuteClick()
    {
        PlayerManager.CmdExecute();
        if (GameManager.GameState != "Surrender")
        {
            GameManager.ChangeScore(false,false);
        }
        PlayerManager.CmdGMChangeState("Initialize {}");
    }

}
