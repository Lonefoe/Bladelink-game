using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SaveSpot : MonoBehaviour
{
    private bool isSaved = false;
    private bool inShrineRange;
    private Animation lightUpAnim;

    private void Awake()
    {
        InputManager.controls.Player.ActionButton.performed += ctx => Save();
        lightUpAnim = GetComponentInChildren<Animation>();
        InputManager.controls.Player.ActionButton.started += ctx => LightUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSaved)
        {
            inShrineRange = true;
            UIManager.Instance.saveTextPopup.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inShrineRange = false;
            UIManager.Instance.saveTextPopup.enabled = false;
        }
    }

    void LightUp()
    {
        lightUpAnim.Play();
    }

    void Save()
    {
        if (!inShrineRange) return;
        Debug.Log("saved");
        isSaved = true;
        UIManager.Instance.saveTextPopup.enabled = false;
        Player.Instance.SetSavePos(Player.Instance.GetPosition());
    }
}
