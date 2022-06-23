using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class surrender_button : MonoBehaviour
{
    public PlayerManager PlayerManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {

        if (NetworkClient.connection.identity.GetComponent<PlayerManager>().IsMyTurn)
        {
            PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            PlayerManager.CmdGMChangeState("Surrender0");
            PlayerManager.ResetCardsPlayed();
        }
    }
}
