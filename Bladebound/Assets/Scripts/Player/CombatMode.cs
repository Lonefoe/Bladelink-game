using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatMode
{
    public Enemy targetEnemy = null;
    private bool inCombatMode;
    

    public void EnterCombatMode()
    {
        inCombatMode = true;
    //    Player.Controller.Face(targetEnemy.gameObject);
        Player.Controller.SetShouldFlip(false);
        Player.Combat.combatCam.gameObject.SetActive(true);
    }

    public void ExitCombatMode()
    {
        inCombatMode = false;
        Player.Controller.SetShouldFlip(true);
        Player.Combat.combatCam.gameObject.SetActive(false);
    }

    public bool IsInCombatMode()
    {
        return inCombatMode;
    }

}
