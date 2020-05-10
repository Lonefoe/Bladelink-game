using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameLevel : Singleton<GameLevel>
{
    public CinemachineBrain mainCamBrain;
    public GameObject mainVirtualCam;
    public GameObject combatCam;
}
