using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private Animator animator;
    private bool slashed;

    void Awake() => animator = GetComponent<Animator>();

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("move");
            AudioManager.Instance.PlayOneShot("GrassMove");
        }
    }

    public void GetSlashed()
    {
        if(slashed) return;
        animator.SetTrigger("slash");
        slashed = true;
        AudioManager.Instance.PlayOneShot("GrassCut");
    }

}
