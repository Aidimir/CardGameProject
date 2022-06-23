using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CombineScript : MonoBehaviour
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

    public void OnClick()
    {
        PlayerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        if (PlayerManager.ReadyToCombine)
        {
            //водород
            //хлор
            string name1 = PlayerManager.PlayerCombinerSockets[0].transform.GetComponentInChildren<PingAbilities>().name;
            string name2 = PlayerManager.PlayerCombinerSockets[1].transform.GetComponentInChildren<PingAbilities>().name;
            if ((name1 == "водород" && name2 == "хлор") || (name1 == "хлор" && name2 == "водород"))
            {
                PlayerManager.CmdCombineCards("хлороводород");
            }
            else if ((name1 == "водород" && name2 == "фтор") || (name1 == "фтор" && name2 == "водород"))
            {
                PlayerManager.CmdCombineCards("фтороводород");
            }
            else if ((name1 == "хлор" && name2 == "фтор") || (name1 == "фтор" && name2 == "хлор"))
            {
                PlayerManager.CmdCombineCards("фторидХлор");
            }
            
            
            
            // else if ((name1 == PlayerManager.Ortofosforn && name2 == "гидроксидНатрия") || (name1 == "гидроксидНатрия" && name2 == PlayerManager.Ortofosforn))
            // {
            //     PlayerManager.CmdCombineCards(PlayerManager.DegidroFas);
            // }
            // else if ((name1 == Player. && name2 == "гидроксидНатрия") || (name1 == "гидроксидНатрия" && name2 == "соляная"))
            // {
            //     PlayerManager.CmdCombineCards(PlayerManager.Hlorid);
            // }
            // else if ((name1 == "соляная" && name2 == "хлор") || (name1 == "хлор" && name2 == "соляная"))
            // {
            //      PlayerManager.CmdCombineCards("хлоридНатрия");
            // }
            // else if ((name1 == "хлоридНатрия" && name2 == "хлор") || (name1 == "хлор" && name2 == "хлоридНатрия"))
            // {
            //     PlayerManager.CmdCombineCards("хлоридНатрия");
            // }
            // else if ((name1 == "гидроксидКалия" && name2 == "соляная") || (name1 == "соляная" && name2 == "гидроксидКалия"))
            // {
            //     PlayerManager.CmdCombineCards(PlayerManager.HloridKal);
            // }
            // else if ((name1 == "гидроксидКалия" && name2 == "ортофосфорная") || (name1 == "ортофосфорная" && name2 == "гидроксидКалия"))
            // {
            //     PlayerManager.CmdCombineCards(PlayerManager.DegidroKov);
            // }
            else
            {
                // PlayerManager.CmdDestroyCombineCards();
                 PlayerManager.CmdCombineCards("бурда");
            }

            PlayerManager.ReadyToCombine = false;
        }
    }
}
