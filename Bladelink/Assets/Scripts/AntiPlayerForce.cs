using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AntiPlayerForce : MonoBehaviour
{
    public float force = 1f;

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if(transform.localPosition.x >= 0f) rb.velocity = new Vector2 (force, rb.velocity.y);
            else if (transform.localPosition.x <= 0f) rb.velocity = new Vector2 (-force, rb.velocity.y);
        }    
    }

}
