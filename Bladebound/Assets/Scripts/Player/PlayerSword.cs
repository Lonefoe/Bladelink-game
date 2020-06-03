using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
	public static PlayerSword Instance { get; private set; }

	// All sword variables
	private List<GameObject> hitEnemies = new List<GameObject>();
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private bool isReturning = false, waitingForReturn = false;
	private int direction = 1;

	private CapsuleCollider2D triggerCollider;
	private CircleCollider2D groundCollider;
	private BoxCollider2D rbCollider;

	private void Awake()
	{
		// Reference setup
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		triggerCollider = GetComponent<CapsuleCollider2D>();
		groundCollider = GetComponent<CircleCollider2D>();
		rbCollider = GetComponentInChildren<BoxCollider2D>();

		Instance = this;
	}

	private void Update()
	{
		// Check we are returning, if yes return the sword to player's position using a lerp
		if (isReturning)
		{
			transform.position = Vector2.Lerp(transform.position, Player.Instance.GetPosition(), 4f * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,-90), 4f * Time.deltaTime);

			// If we're close to the player, stop returning and make player get the sword in hands again
			if (Vector2.Distance(transform.position, Player.Instance.GetPosition()) < 0.5f)
			{
				isReturning = false;
				waitingForReturn = false;
				Destroy(gameObject);
				Instance = null;
			}
		}
	}

	// Checking if we're slow enough to return to player
	private void FixedUpdate()
	{
		if (!isReturning && !waitingForReturn)
		{
			if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 1f)
			{
				Player.Combat.CanReturn = true;
				waitingForReturn = true;
			}
		}
		else if (!waitingForReturn) Player.Combat.CanReturn = false;
	}

	// Called from player combat script, applies force and throws itself
	public void Throw(Vector2 throwForce)
	{
		isReturning = false;

		// If the input is moving the sword right and the sword is facing left...
		if (throwForce.x > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the sword left and the sword is facing right...
		else if (throwForce.x < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}

		m_Rigidbody2D.isKinematic = false;
		m_Rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);

	}

	// Return to player when called
	public void Return()
	{
		isReturning = true;
		hitEnemies.Clear();
		m_Rigidbody2D.isKinematic = true;
		// m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // Freeze rotation only => unfreeze position
		m_Rigidbody2D.Sleep();

		triggerCollider.enabled = false;
		groundCollider.enabled = false;
	}

	// Register when we hit enemy
	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Enemy" && !hitEnemies.Contains(other.gameObject))
		{
			Player.Combat.HitEnemy(other, false);
			hitEnemies.Add(other.gameObject);
			m_Rigidbody2D.AddTorque(120 * -direction, ForceMode2D.Force);
			m_Rigidbody2D.drag = 20;

			triggerCollider.enabled = false;
			groundCollider.enabled = true;
			rbCollider.enabled = false;

			waitingForReturn = true;
			Player.Combat.CanReturn = true;
		}
	}

	// Check whether we're touching the ground, if yes, stop the sword
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (groundCollider.enabled == true && collision.gameObject.tag == "Ground" && !m_Rigidbody2D.isKinematic)
		{
			m_Rigidbody2D.isKinematic = true;
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			triggerCollider.enabled = false;
			AudioManager.Instance.Play("SwordGround");
		}
	}

	// Flip the sword based on the direction of it's throw
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		direction = -direction;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;

		GetComponentInChildren<ParticleSystem>().gameObject.transform.localScale = theScale;
	}

}
