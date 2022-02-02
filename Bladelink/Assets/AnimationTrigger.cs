using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{

    public Animation anim;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            Player.Movement.DisableMovement();
            Invoke("PlayAnim", 1.2f);
        }
    }

    void PlayAnim()
    {
        if(anim.gameObject.activeInHierarchy == false) anim.gameObject.SetActive(true);
        Invoke("ExitGame", 12f);
    }

    void ExitGame()
    {
        GetComponent<Level>().ExitGame();
    }

}
