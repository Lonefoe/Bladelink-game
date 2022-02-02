using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SaveSpot : MonoBehaviour
{
    private bool isSaved = false;
    private bool inShrineRange;
    private Animation lightUpAnim;
    private ImagePopup imagePopup;

    private void Awake()
    {
        InputManager.controls.Player.ActionButton.performed += ctx => Save();
        lightUpAnim = GetComponentInChildren<Animation>();
        imagePopup = GetComponentInChildren<ImagePopup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSaved)
        {
            inShrineRange = true;
            UIManager.Instance.saveTextPopup.enabled = true;
            imagePopup.ShrineRangeEntered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inShrineRange = false;
            UIManager.Instance.saveTextPopup.enabled = false;
            imagePopup.ShrineRangeExited();
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
        LightUp();
        isSaved = true;
        UIManager.Instance.saveTextPopup.enabled = false;
        imagePopup.ShrineRangeExited();
        Player.Instance.RestoreHealth(Player.Instance.Stats.maxHealth);
        Player.Instance.SetSavePos(Player.Instance.GetPosition());
        AudioManager.Instance.PlayOneShot("Save");
    }
}
