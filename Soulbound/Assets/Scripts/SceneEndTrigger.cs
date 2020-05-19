using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.Movement.DoRunToggle(0.5f);
            GameManager.Instance.Invoke("LoadNextLevel", 1.2f);
        }
    }
}
