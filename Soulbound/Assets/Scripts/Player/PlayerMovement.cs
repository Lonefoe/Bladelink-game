using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    private float moveMultiplier = 1f;
    private float horizontalMove = 0f;
    private int direction = 1;

    private void Awake()
    {
        InputManager.controls.Player.Jump.started += ctx => Player.Controller.Jump();
        InputManager.controls.Player.Jump.canceled += ctx => Player.Controller.CutJump();
    }

    // We handle everything input-based in here
    void Update()
    {
        horizontalMove = InputManager.Instance.moveInput.x * moveSpeed * moveMultiplier;        // Getting our mov. input and boosting that input with our speed variable

        Player.Anim.SetFloat("Speed", Mathf.Abs(horizontalMove));          // Transfers our input to Speed variable inside of animator
        Player.Anim.SetBool("isInAir", !Player.Controller.m_Grounded);     // We take the grounded property from controller and send it to animator

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
