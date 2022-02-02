using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private ImagePopup imagePopup;
    private bool pickedUp, inRange;

    // Start is called before the first frame update
    private void Awake() {
        InputManager.controls.Player.ActionButton.performed += ctx => PickUp();
        imagePopup = GetComponentInChildren<ImagePopup>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        imagePopup.ShrineRangeEntered();
        inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        imagePopup.ShrineRangeExited();
        inRange = false;
    }

    private void PickUp()
    {
        if (pickedUp || !inRange) return;
        pickedUp = true;
        Player.Instance.HasStatue = true;
        gameObject.SetActive(false);
    }

}
