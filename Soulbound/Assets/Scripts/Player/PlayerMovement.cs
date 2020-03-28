﻿using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 40f;
    private float horizontalMove = 0f;
    private bool jump = false;

    // We handle everything input-based in here
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;    // Storing our mov. input and boosting that input with our speed variable

        if (Input.GetButtonDown("Jump"))                                // Jump logic
        {
            jump = true;

        }

        Player.Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));          // Transfers our input to Speed variable inside of animator
        Player.Animator.SetBool("isInAir", !Player.Controller.m_Grounded);            // We take the grounded property from controller and send it to animator

    }

    // Where we call the movement logic from the controller
    void FixedUpdate()
    {
        Player.Controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);     // We call for a function from the character controller
        jump = false;                                                          // Resetting the jump variable

    }

}
