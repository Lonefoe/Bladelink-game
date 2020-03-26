using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    CharacterController controller;
    Animator animator;

    [SerializeField] private float moveSpeed = 25f;
    private int direction = 1;

    // Reference setup
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

    }

    private void FixedUpdate()
    {
        if (direction == 1)
        {
            controller.Move(moveSpeed * Time.fixedDeltaTime, false, false);
        }
        else if (direction == -1)
        {
            controller.Move(-moveSpeed * Time.fixedDeltaTime, false, false);
        }
    }

    public void ChangeDirection(Vector2 targetPos)
    {
        if (transform.position.x <= targetPos.x)
        {
            direction = 1;
        }
        else direction = -1;

    }

}
