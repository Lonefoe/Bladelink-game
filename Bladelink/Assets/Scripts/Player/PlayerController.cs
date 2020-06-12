using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    [Header("Player Controller")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Vector2 ledgeClimbOffset = Vector2.zero, ledgeOffsetStart;
    protected bool isTouchingWall, isTouchingLedge;
    private bool ledgeDetected, holdingLedge, ledgeJumped;
    private bool ignoreCheck;
    private float fJumpPressedRemember, fGroundedRemember;
    [SerializeField] float fJumpPressedRememberTime, fGroundedRememberTime;
    [Range(0,1)][SerializeField] float fCutJumpHeight;
    [SerializeField] float ledgeJumpMultiplier = 1f;
    private bool jumpKeyDown;

    private Vector2 ledgePosBot, ledgePos;

    private void Start()
    {
        base.OnLandEvent.AddListener(OnLand);
        ledgeOffsetStart = ledgeClimbOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.Instance.IsDead() || GameManager.Instance.IsGamePaused()) return;

        fJumpPressedRemember -= Time.deltaTime;
        fGroundedRemember -= Time.deltaTime;
        if (m_Grounded) fGroundedRemember = fGroundedRememberTime;

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

        if (fJumpPressedRemember > 0 && fGroundedRemember > 0)
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0f;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce); // Jump
            if(!jumpKeyDown) CutJump();
            EffectsManager.Instance.SpawnParticles("SmokePuff", m_GroundCheck.position + new Vector3(0, 0.35f, 0));
            Player.Instance.PlayFootstep();
        } 
        else if (fJumpPressedRemember > 0 && IsClimbingLedge() && !ledgeJumped)
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0f;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * ledgeJumpMultiplier); // Ledge jump
            if(!jumpKeyDown) CutJump();
            LedgeJumped();
        }
    }

    // When we press jump button
    public override void Jump()
    {
        fJumpPressedRemember = fJumpPressedRememberTime;
        jumpKeyDown = true;
    }

    // When we release jump button
    // We cut jump's height
    public void CutJump()
    {
        if (m_Rigidbody2D.velocity.y > 0)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * fCutJumpHeight); // Cut jump
        }

        jumpKeyDown = false;
    }

    void OnLand()
    {
        EffectsManager.Instance.SpawnParticles("SmokePuff", m_GroundCheck.position + new Vector3(0, 0.2f, 0));
    }

    //=====================================================
    // Ledge climbing
    //=====================================================  

    private void CheckSurroundings()
    {  
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right * Player.Movement.GetDirection(), 0.4f, m_WhatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right * Player.Movement.GetDirection(), 0.4f, m_WhatIsGround);

        if (isTouchingWall && !isTouchingLedge & !ledgeDetected && !ignoreCheck)
        {
            // Checks if ground's close to the player so that he won't get bugged in it
            bool isGroundClose = Physics2D.Raycast(m_GroundCheck.position, transform.up * -1, 0.5f, m_WhatIsGround);
            if (!isGroundClose)
            {
                ledgeDetected = true;
                ledgePosBot = wallCheck.position;
            }
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

            // Checking if it's an object that we want to climb
            Collider2D ledgeCol = Physics2D.OverlapCircle(new Vector2(wallCheck.position.x + 0.4f * Player.Movement.GetDirection(), wallCheck.position.y), 0.4f, m_WhatIsGround);
            if (ledgeCol != null && ledgeCol.gameObject.CompareTag("Ledge"))
            {
                ledgeClimbOffset = ledgeOffsetStart + ledgeCol.GetComponent<Ledge>().UpdateLedgeOffset();
            } else ledgeClimbOffset = ledgeOffsetStart;

            if (IsFacingRight())
            {
                ledgePos = new Vector2(Mathf.Floor(ledgePosBot.x + 0.5f) - ledgeClimbOffset.x, Mathf.Floor(ledgePosBot.y) + ledgeClimbOffset.y);
            }
            else
            {
                ledgePos = new Vector2(Mathf.Ceil(ledgePosBot.x - 0.5f) + ledgeClimbOffset.x, Mathf.Floor(ledgePosBot.y) + ledgeClimbOffset.y);
            }

            transform.position = ledgePos;
            FreezePosition(true);

            m_canMove = false;
            SetShouldFlip(false);
            Player.Anim.SetBool("holdingLedge", holdingLedge);
        }

        if (holdingLedge && InputManager.Instance.moveInput.y <= -1f)
        {
                LedgeJumped();
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
        m_Rigidbody2D.gravityScale = 2f;
        FreezePosition(false);
        ledgeJumped = true;
        holdingLedge = false;
        ignoreCheck = true;
        m_canMove = true;
        SetShouldFlip(true);
        Player.Anim.SetBool("holdingLedge", holdingLedge);
    }
    
    // We use this when we want player in one place
    public void FreezePosition(bool freeze, bool onlyX = false)
    {
        if (freeze & !onlyX) m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        else if (freeze & onlyX) m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public Rigidbody2D GetRigidbody()
    {
        return m_Rigidbody2D;
    }

    public bool IsClimbingLedge() { return holdingLedge; }
    public bool IsTouchingWall() { return isTouchingWall; }
    public bool IsTouchingLedge() { return isTouchingLedge; }
}
