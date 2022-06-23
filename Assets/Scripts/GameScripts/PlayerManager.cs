using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{
    public GameManager GameManager;
    public GameObject Ping;
    public GameObject Burda;
    public GameObject DegidroFas;
    public GameObject DegidroKov;
    public GameObject Ftor;
    public GameObject FtoridHlora;
    public GameObject Ftorovodorod;
    public GameObject Hlor;
    public GameObject Hlorid;
    public GameObject HloridFtor;
    public GameObject HloridKal;
    public GameObject Hlorovodorod;
    public GameObject Ortofosforn;
    public GameObject Solyan;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject PlayerSlot1;
    public GameObject PlayerSlot2;
    public GameObject PlayerSlot3;
    public GameObject PlayerSlot4;
    public GameObject PlayerSlot5;
    public GameObject EnemySlot1;
    public GameObject EnemySlot2;
    public GameObject EnemySlot3;
    public GameObject EnemySlot4;
    public GameObject EnemySlot5;
    public GameObject PCombineSlot1;
    public GameObject PCombineSlot2;
    public GameObject ECombineSlot1;
    public GameObject ECombineSlot2;
    public GameObject PlayerYard;
    public GameObject EnemyYard;
    Color blueColor = new Color32(17, 216, 238, 255);
    public bool ReadyToCombine = false;
    public List<GameObject> PlayerSockets = new List<GameObject>();
    public List<GameObject> PlayerCombinerSockets = new List<GameObject>();

    public List<GameObject> EnemySockets = new List<GameObject>();
    public List<GameObject> EnemyCombinerSockets = new List<GameObject>();
    public bool FirstCombineSocketIsBusy = false;
    public bool SecondCombineSocketIsBusy = false;
    public bool FirstEnemyCombineSocketIsBusy = false;
    public bool SecondEnemyCombineSocketIsBusy = false;
    public int CardsPlayed = 0;
    public bool IsMyTurn = false;
    public bool combine = false;
    public List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        PlayerYard = GameObject.Find("PlayerYard");
        EnemyYard = GameObject.Find("EnemyYard");

        PlayerSlot1 = GameObject.Find("PlayerSlot1");
        PlayerSlot2 = GameObject.Find("PlayerSlot2");
        PlayerSlot3 = GameObject.Find("PlayerSlot3");
        PlayerSlot4 = GameObject.Find("PlayerSlot4");
        PlayerSlot5 = GameObject.Find("PlayerSlot5");
        EnemySlot1 = GameObject.Find("EnemySlot1");
        EnemySlot2 = GameObject.Find("EnemySlot2");
        EnemySlot3 = GameObject.Find("EnemySlot3");
        EnemySlot4 = GameObject.Find("EnemySlot4");
        EnemySlot5 = GameObject.Find("EnemySlot5");
        PCombineSlot1 = GameObject.Find("PCombineSlot1");
        PCombineSlot2 = GameObject.Find("PCombineSlot2");
        ECombineSlot1 = GameObject.Find("ECombineSlot1");
        ECombineSlot2 = GameObject.Find("ECombineSlot2");
        PlayerCombinerSockets.Add(PCombineSlot1);
        PlayerCombinerSockets.Add(PCombineSlot2);
        EnemyCombinerSockets.Add(ECombineSlot1);
        EnemyCombinerSockets.Add(ECombineSlot2);
        PlayerSockets.Add(PlayerSlot1);
        PlayerSockets.Add(PlayerSlot2);
        PlayerSockets.Add(PlayerSlot3);
        PlayerSockets.Add(PlayerSlot4);
        PlayerSockets.Add(PlayerSlot5);
        EnemySockets.Add(EnemySlot1);
        EnemySockets.Add(EnemySlot2);
        EnemySockets.Add(EnemySlot3);
        EnemySockets.Add(EnemySlot4);
        EnemySockets.Add(EnemySlot5);

        if (isClientOnly)
        {
            IsMyTurn = true;
        }
    }

    [Server]
    public override void OnStartServer()
    {
        cards.Add(Ping);
        cards.Add(Burda);
        cards.Add(DegidroFas);
        cards.Add(DegidroKov);
        cards.Add(Ftor);
        cards.Add(FtoridHlora);
        cards.Add(Ftorovodorod);
        cards.Add(Hlor);
        cards.Add(Hlorid);
        cards.Add(HloridFtor);
        cards.Add(HloridKal);
        cards.Add(Hlorovodorod);
        cards.Add(Ortofosforn);
        cards.Add(Solyan);

    }

    [Command]
    public void CmdDealCards()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
        GameObject h = Instantiate(cards[7], new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(h, connectionToClient);
        GameObject p = Instantiate(cards[0], new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(p, connectionToClient);
        RpcShowCard(h, "Dealt");
        RpcShowCard(p, "Dealt");

        RpcGMChangeState("Compile {}");
    }
    
    public void PlayCard(GameObject card)
    {
        card.GetComponent<CardAbilities>().OnCompile();
        Debug.Log("PlayCard");
        CmdPlayCard(card);
    }

    public void ShowCard(GameObject card)
    { 
        Debug.Log("ShowCard");
        CmdShowCard(card);
    }
    [Command]
    void CmdPlayCard(GameObject card)
    {
        Debug.Log("CmdPlayCard");
        RpcShowCard(card, "Played");
    }

    [Command]
    void CmdShowCard(GameObject card)
    {
        Debug.Log("CmdShowCard");
        RpcShowCard(card, "Show combo");
    }
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        Debug.Log("RpcShowCard");
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
                card.GetComponent<CardFlipper>().SetSprite("cyan");
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
                card.GetComponent<CardFlipper>().SetSprite("magenta");
                card.GetComponent<CardFlipper>().Flip();
            }
        }
        else if (type == "Played")
        {
            if (CardsPlayed == 5)
            {
                CardsPlayed = 0;
            }
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerSockets[CardsPlayed].transform, false);
                CmdGMCardPlayed();
            }
            if (!hasAuthority)
            {
                card.transform.SetParent(EnemySockets[4-CardsPlayed].transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
            CardsPlayed++;
            PlayerManager pm = NetworkClient.connection.identity.GetComponent<PlayerManager>();
            pm.IsMyTurn = !pm.IsMyTurn;
        }
        else if (type == "Show combo")
        {
            if (hasAuthority)
            {
                if (FirstCombineSocketIsBusy == false)
                {
                    card.transform.SetParent(PlayerCombinerSockets[0].transform, false);
                    FirstCombineSocketIsBusy = true;
                }
                else
                {
                    if (SecondCombineSocketIsBusy == false)
                    {
                        card.transform.SetParent(PlayerCombinerSockets[1].transform, false);
                        SecondCombineSocketIsBusy = true;
                        ReadyToCombine = true;
                    }
                    else
                    {
                        return;
                    }
                }
                Debug.Log("HAS AUTHORITY MESSAGE");
            }
            else
            {
                if (FirstEnemyCombineSocketIsBusy == false)
                {
                    card.transform.SetParent(EnemyCombinerSockets[0].transform, false);
                    FirstEnemyCombineSocketIsBusy = true;
                }
                else
                {
                    if (SecondEnemyCombineSocketIsBusy == false)
                    {
                        card.transform.SetParent(EnemyCombinerSockets[1].transform, false);
                        SecondEnemyCombineSocketIsBusy = true;
                        ReadyToCombine = true;
                    }
                }
                Debug.Log("DOESN't HAS AUTHORITY MESSAGE");
            }
        }
    }

    [Command]
    public void CmdGMChangeState(string stateRequest)
    {
        RpcGMChangeState(stateRequest);
    }

    [ClientRpc]
    void RpcGMChangeState(string stateRequest)
    {
        GameManager.ChangeGameState(stateRequest);
        if (stateRequest == "Compile {}")
        {
            GameManager.ChangeReadyClicks();
        }
    }

    [Command]
    void CmdGMCardPlayed()
    {
        RpcGMCardPlayed();
    }

    [ClientRpc]
    void RpcGMCardPlayed()
    {
        GameManager.CardPlayed();
    }

    [Command]
    public void CmdExecute()
    {
        RpcExecute();
    }

    [ClientRpc]
    void RpcExecute()
    {
        for (int i = 0; i < PlayerSockets.Count; i++)
        {
            PlayerSockets[i].transform.GetComponentInChildren<CardAbilities>().OnExecute();
            PlayerSockets[i].transform.GetChild(0).gameObject.transform.SetParent(PlayerYard.transform, false);
            EnemySockets[i].transform.GetChild(0).gameObject.transform.SetParent(EnemyYard.transform, false);
        }

        CardsPlayed = 0;
    }

    [Command]
    public void CmdGMChangeVariables(int variables)
    {
        RpcGMChangeVariables(variables);
    }

    [ClientRpc]
    public void RpcGMChangeVariables(int variables)
    {
        GameManager.ChangeVariables(variables, hasAuthority);
    }

    [Command]
    public void CmdGMChangeBP(int playerBP, int opponentBP)
    {
        RpcGMChangeBP(playerBP, opponentBP);
    }

    [ClientRpc]
    public void RpcGMChangeBP(int playerBP, int opponentBP)
    {
        GameManager.ChangeBP(playerBP, opponentBP, hasAuthority);
    }

    public void ResetCardsPlayed()
    {
        CmdResetCardsPlayed();
    }

    [Command]
    public void CmdResetCardsPlayed()
    {
        RpcResetCardsPlayed();
    }

    [ClientRpc]
    public void RpcResetCardsPlayed()
    {
        CardsPlayed = 0;
    }
    
    
    [Command]
    public void CmdCombineCards(string name)
    {
        Debug.Log("RPCCOMBINECARDS");
        if(name == "хлороводород"){
        GameObject card = Instantiate(cards[11], new Vector2(0, 0), Quaternion.identity);
        NetworkServer.Spawn(card, connectionToClient);
        RpcShowCard(card, "Dealt");
        }
        else if(name == "фтороводород"){
            GameObject card = Instantiate(cards[6], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
        else if(name == "фторидХлор"){
            GameObject card = Instantiate(cards[5], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
        else if(name == "бурда"){
            GameObject card = Instantiate(cards[1], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
        RpcDestroyCombineCards();
    }

    [Command]
    public void CmdDestroyCombineCards()
    {
        RpcDestroyCombineCards();
    }
    [ClientRpc]
    public void RpcDestroyCombineCards()
    {
        if (hasAuthority)
        {
            FirstCombineSocketIsBusy = false;
            SecondCombineSocketIsBusy = false;
            Destroy(PlayerCombinerSockets[0].transform.GetChild(0).gameObject);
            Destroy(PlayerCombinerSockets[1].transform.GetChild(0).gameObject);
        }
        else
        {
            FirstEnemyCombineSocketIsBusy = false;
            SecondEnemyCombineSocketIsBusy = false;
            Destroy(EnemyCombinerSockets[0].transform.GetChild(0).gameObject);
            Destroy(EnemyCombinerSockets[1].transform.GetChild(0).gameObject);
        }
    }
}
