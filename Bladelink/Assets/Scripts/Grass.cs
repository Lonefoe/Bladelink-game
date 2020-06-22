﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private Animator animator;

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
        animator.SetTrigger("slash");
        AudioManager.Instance.PlayOneShot("GrassCut");
    }

}