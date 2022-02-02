using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianStone : MonoBehaviour
{

    public GameObject guardian;
    private bool inRange, gateOpened;

    private void Awake() {
        InputManager.controls.Player.ActionButton.performed += ctx => OpenGate();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() && other.gameObject.GetComponent<Player>().HasStatue)
        {
            if(gateOpened) return;
            GetComponentInChildren<ImagePopup>().ShrineRangeEntered();
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>())
        {
            GetComponentInChildren<ImagePopup>().ShrineRangeExited();
            inRange = false;
        }
    }

    private void OpenGate()
    {
        if (!inRange) return;
        guardian.GetComponent<Animation>().Play();
        AudioManager.Instance.Play("GuardianMove");
    }

}
