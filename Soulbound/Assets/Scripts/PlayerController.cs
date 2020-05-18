using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    [Header("Player Controller")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Vector2 ledgeClimbOffset1 = Vector2.zero;
    protected bool isTouchingWall, isTouchingLedge;
    private bool ledgeDetected, holdingLedge, ledgeJumped;
    private bool ignoreCheck;
    private float fJumpPressedRemember;
    [SerializeField] float fJumpPressedRememberTime;
[Range(0,1)][SerializeField] float fCutJumpHeight;
    [SerializeField] float ledgeJumpMultiplier = 1f;

    private Vector2 ledgePosBot, ledgePos;

    // Update is called once per frame
    void Update()
    {
        fJumpPressedRemember -= Time.deltaTime;

        CheckSurroundings();

        if (ledgeJumped) return;
        CheckLedgeClimb();
    }

    //=====================================================
    // Jump stuff
    //=====================================================  

    // Checking for jumping & applying forces
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (fJumpPressedRemember > 0 && m_Grounded)
        {
            fJumpPressedRemember = 0;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce); // Jump
        } 
        else if (fJumpPressedRemember > 0 && IsClimbingLedge() && !ledgeJumped)
        {
            fJumpPressedRemember = 0;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * ledgeJumpMultiplier); // Ledge jump
            LedgeJumped();
        }
    }

    // When we press jump button
    public override void Jump()
    {
        fJumpPressedRemember = fJumpPressedRememberTime;
    }

    // When we release jump button
    // We cut jump's height
    public void CutJump()
    {
        if (m_Rigidbody2D.velocity.y > 0 && !ledgeJumped)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * fCutJumpHeight); // Cut jump
        }
    }

    //=====================================================
    // Ledge climbing
    //=====================================================  

    private void CheckSurroundings()
    {  
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right * Player.Movement.GetDirection(), 0.5f, m_WhatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right * Player.Movement.GetDirection(), 0.5f, m_WhatIsGround);


        if (isTouchingWall && !isTouchingLedge & !ledgeDetected && !ignoreCheck)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        } 
        else if (!isTouchingWall && !isTouchingLedge && ignoreCheck)
        {
            FinishLedgeClimb();
        }
    }

    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !holdingLedge && !ledgeJumped)
        {
            holdingLedge = true;

            if (IsFacingRight())
            {
                ledgePos = new Vector2(Mathf.Floor(ledgePosBot.x + 0.5f) - ledgeClimbOffset1.x, Mathf.Floor(ledgePosBot.y) + ledgeClimbOffset1.y);
            }
            else
            {
                ledgePos = new Vector2(Mathf.Ceil(ledgePosBot.x - 0.5f) + ledgeClimbOffset1.x, Mathf.Floor(ledgePosBot.y) + ledgeClimbOffset1.y);
            }

            m_canMove = false;
            SetShouldFlip(false);
            Player.Anim.SetBool("holdingLedge", holdingLedge);
        }

        if (holdingLedge)
        {
            transform.position = ledgePos;
        }
    }

    

    public void FinishLedgeClimb()
    {
        if (ledgeJumped)
        {
            ledgeJumped = false;
            ignoreCheck = false;
            ledgeDetected = false;
        }
    }

    public void LedgeJumped()
    {
        ledgeJumped = true;
        holdingLedge = false;
        ignoreCheck = true;
        m_canMove = true;
        SetShouldFlip(true);
        Player.Anim.SetBool("holdingLedge", holdingLedge);
    }

    public bool IsClimbingLedge() { return holdingLedge; }
    public bool IsTouchingWall() { return isTouchingWall; }
    public bool IsTouchingLedge() { return isTouchingLedge; }
}
