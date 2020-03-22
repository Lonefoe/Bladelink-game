using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References
    CharacterController controller;
    Animator animator;

    // Properties
    [SerializeField] private float moveSpeed = 40f;
    private float horizontalMove = 0f;
    private bool jump = false;


    private void Start()
    {
            // Set our references
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

        // We handle everything input-based in here
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;    // Storing our mov. input and boosting that input with our speed variable

        if (Input.GetButtonDown("Jump"))                                // Jump logic
        {
            jump = true;

        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));          // Transfers our input to Speed variable inside of animator
        animator.SetBool("isInAir", !controller.m_Grounded);            // We take the grounded property from controller and send it to animator
        
    }

    public void OnLand ()
    {
        animator.SetBool("isInAir", false);     // We call for the animator to say that we're no longer in air (we call it as an event from the controller script)
    }


        // Where we call the movement logic from the controller
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);     // We call for a function from the character controller
        jump = false;                                                          // Resetting the jump variable

    }


}
