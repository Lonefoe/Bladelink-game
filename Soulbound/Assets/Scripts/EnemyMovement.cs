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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

    }


    void FixedUpdate()
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

    public void ChangeDirection(int dir)
    {
        direction = dir;
    }

}
