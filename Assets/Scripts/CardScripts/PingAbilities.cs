using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PingAbilities : CardAbilities
{
    public string name;
    public int power;
    public override void OnCompile()
    {
        PlayerManager.CmdGMChangeVariables(1);
        PlayerManager.CmdGMChangeBP(power, 0);
    }

    public override void OnExecute()
    {
    }
}
