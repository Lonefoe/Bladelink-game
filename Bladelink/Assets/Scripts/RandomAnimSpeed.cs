using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimSpeed : MonoBehaviour
{
    Animator animator;

    public float maxSpeed, minSpeed;
    public float updateTime = 4f;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InvokeRepeating("MakeRandomSpeed", 0f, updateTime);
    }

    void MakeRandomSpeed()
    {
        animator.SetFloat("RandomSpeed", Random.Range(minSpeed, maxSpeed));
    }
}
