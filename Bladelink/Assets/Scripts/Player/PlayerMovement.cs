using UnityEngine;
using Utilities;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    private float moveMultiplier = 1f;
    private float horizontalMove = 0f;
    private bool doRun = false;
    private int direction = 1;

    private void Awake()
    {
        InputManager.controls.Player.Jump.started += ctx => Player.Controller.Jump();
        InputManager.controls.Player.Jump.canceled += ctx => Player.Controller.CutJump();
    }

    // We handle everything input-based in here
    void Update()
    {
        if(GameManager.Instance.IsGamePaused()) return;

        if (!doRun) horizontalMove = InputManager.Instance.moveInput.x * moveSpeed * moveMultiplier;        // Getting our mov. input and boosting that input with our speed variable
        else horizontalMove = moveSpeed * moveMultiplier;

        Player.Anim.SetFloat("Speed", Mathf.Abs(horizontalMove));          // Transfers our input to Speed variable inside of animator
        Player.Anim.SetBool("isInAir", !Player.Controller.m_Grounded);     // We take the grounded property from controller and send it to animator

        int airState = 0;
        if(Utils.IsBetween(Player.Controller.GetRigidbody().velocity.y, 1f, -6f)) airState = 1;
        else if (Player.Controller.GetRigidbody().velocity.y > 1f) airState = 0;
        else if (Player.Controller.GetRigidbody().velocity.y < -6f) airState = 2;
        

        Player.Anim.SetInteger("airState", airState);

        if (Player.Controller.IsFacingRight()) direction = 1;
        else direction = -1;

    }

    // Where we call the movement logic from the controller
    void FixedUpdate()
    {
        Player.Controller.Move(horizontalMove * Time.fixedDeltaTime, false);     // We call for a function from the character controller
    }

    public int GetDirection()
    {
        return direction;
    }

    public void Knockback(float force)
    {
        Player.Rigidbody.AddForce(Vector2.right * GetDirection() * -force * Player.Rigidbody.mass);
    }

    public void DoRunToggle(float moveMult = 1f)
    {
        doRun = !doRun;
        if (doRun) moveMultiplier = moveMult;
        else moveMultiplier = 1f;
    }

    public void SetMoveMultiplier(float value)
    {
        moveMultiplier = value;
    }

    public void DisableMovement()
    {
        Player.Rigidbody.velocity = Vector2.zero;
        enabled = false;
    }

    public void EnableMovement()
    {
        enabled = true;
    }

}
