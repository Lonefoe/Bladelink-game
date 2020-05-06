using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	public static Sword Instance { get; private set; }

	// All sword variables
	private List<GameObject> hitEnemies = new List<GameObject>();
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private bool isReturning = false;
	private bool waitingForReturn = false;
	private int direction = 1;

	private void Awake()
	{
		// Reference setup
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		Instance = this;
	}

	private void Update()
	{
		// Check we are returning, if yes return the sword to player's position using a lerp
		if (isReturning)
		{
			transform.position = Vector2.Lerp(transform.position, Player.Instance.GetPosition(), 4f * Time.deltaTime);

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

	private void FixedUpdate()
	{
		//
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

	public void Throw(Vector2 throwForce)
	{
		isReturning = false;

		// If the input is moving the player right and the player is facing left...
		if (throwForce.x > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (throwForce.x < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}

		m_Rigidbody2D.isKinematic = false;
		m_Rigidbody2D.AddForce(throwForce, ForceMode2D.Impulse);

	}

	public void Return()
	{
		isReturning = true;
		hitEnemies.Clear();
		m_Rigidbody2D.isKinematic = true;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // Freeze rotation only => unfreeze position
		m_Rigidbody2D.Sleep();

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy" && !hitEnemies.Contains(other.gameObject))
		{
			Player.Combat.HitEnemy(other);
			hitEnemies.Add(other.gameObject);
			m_Rigidbody2D.isKinematic = true;
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;

			waitingForReturn = true;
			Player.Combat.CanReturn = true;
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}

}
