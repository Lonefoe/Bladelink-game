using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class CharacterController : MonoBehaviour
{
	[Header("Character Base")]
	[SerializeField] protected float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float fallMultiplier = 2.5f;
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private bool shouldFlip = true;
	[SerializeField] protected LayerMask m_WhatIsGround;                         // A mask determining what is ground to the character
	[SerializeField] protected Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .12f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded { get; set; }          // Whether or not the player is grounded.
	protected bool wasGrounded;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	protected Rigidbody2D m_Rigidbody2D;
	protected SpriteRenderer m_SpriteRenderer;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	protected bool m_Strafing = false;
	protected Transform[] children;
	private Vector3 m_Velocity = Vector3.zero;

	protected bool m_canMove = true;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;
	protected SurfaceType currentSurface;

	 private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
		children = GetComponentsInChildren<Transform>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	protected virtual void FixedUpdate()
	{
		wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		
		// NormalizeSlope();
	}


	public void Move(float move, bool crouch)
	{
		if(GameManager.Instance.IsGamePaused()) return;
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			if (m_canMove)
			{
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			}

			if (shouldFlip && !m_Strafing)
			{
				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			}
		}
		
	}

	public virtual void Jump()
	{
		// If the player should jump...
		if (m_Grounded)
		{
			m_Rigidbody2D.velocity = Vector2.up * m_JumpForce;
		}
	}

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
		foreach(Transform child in children)
		{
			if(child != gameObject.transform)
			{
			Vector3 pos = child.localPosition;
        	pos.x *= -1;
       		child.localPosition = pos;
			}
		}
	}

	void NormalizeSlope()
	{
		// Attempt vertical normalization
		if (m_Grounded)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, m_WhatIsGround);

			if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
			{
				var slopeFriction = 2f;
				// Apply the opposite force against the slope force 
				// You will need to provide your own slopeFriction to stabalize movement
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - (hit.normal.x * slopeFriction), m_Rigidbody2D.velocity.y);

				//Move Player up or down to compensate for the slope below them
				Vector3 pos = transform.position;
				pos.y += -hit.normal.x * Mathf.Abs(m_Rigidbody2D.velocity.x) * Time.deltaTime * (m_Rigidbody2D.velocity.x - hit.normal.x > 0 ? 1 : -1);
				transform.position = pos;
			}
		}
	}

	public void Launch(float launchForce)
	{
		if (m_FacingRight)
		{
			m_Rigidbody2D.AddForce(new Vector2(launchForce, 0f), ForceMode2D.Force);
		}
		else
		{
			m_Rigidbody2D.AddForce(new Vector2(-launchForce, 0f), ForceMode2D.Force);
		}
	}

	public int GetDir()
	{
		if(IsFacingRight()) return 1;
		else return -1;
	}

	public bool IsFacingRight()
	{
		return m_FacingRight;
	}

	public void Face(GameObject obj)
	{
		if (transform.position.x <= obj.transform.position.x)
		{
			if (!m_FacingRight) Flip();
		}
		else
		{
			if (m_FacingRight) Flip();
		}
	}

	public void SetShouldFlip(bool flip)
	{
		shouldFlip = flip;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
		Gizmos.color = Color.green;
	}

}
