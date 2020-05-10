using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatMode
{
    public CinemachineVirtualCamera combatCam;
    public Enemy targetEnemy = null;
    private bool inCombatMode;
    

    public void EnterCombatMode()
    {
        inCombatMode = true;
        Player.Controller.Face(targetEnemy.gameObject);
        Player.Controller.SetShouldFlip(false);
    }

    public void ExitCombatMode()
    {
        inCombatMode = false;
        Player.Controller.SetShouldFlip(true);
    }

    public bool IsInCombatMode()
    {
        return inCombatMode;
    }

}
