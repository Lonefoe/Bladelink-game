using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] private float moveSpeed = 25f;
    private bool disabled = false;
    private bool stopped = false;
    public float moveInput = 0f;
    private float horizontalMove;
    private int direction = 1;


    // Reference setup
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemy.Attack.onSlashEvent += DisableMovement;
    }

    void Update()
    {
        horizontalMove = moveInput * moveSpeed;
        enemy.Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        if (enemy.IsDead() || disabled) { horizontalMove = 0; enemy.Controller.Move(0, false); return; }

        if (direction == 1)
        {
            enemy.Controller.Move(horizontalMove * Time.fixedDeltaTime, false);
        }
        else if (direction == -1)
        {
            enemy.Controller.Move(-horizontalMove * Time.fixedDeltaTime, false);
        }
    }

    public void UpdateDirection(Vector2 targetPos)
    {
        if (transform.position.x <= targetPos.x)
        {
            direction = 1;
        }
        else direction = -1;
    }

    	public void Face(GameObject obj)
	{
		if (transform.position.x <= obj.transform.position.x)
		{
			if (!enemy.Controller.IsFacingRight()) { enemy.Controller.Flip(); direction = 1; }
		}
		else
		{
			if (enemy.Controller.IsFacingRight()) { enemy.Controller.Flip(); direction = -1; }
		}
	}

    public int GetDirection()
    {
        return direction;
    }

    public void Flip()
    {
        direction = -direction;
    }

    public void Knockback(float force = 580f, bool attackCalled = false)
    {
        if (attackCalled) enemy.Rigidbody.AddForce(Vector2.right * Player.Movement.GetDirection() * force * enemy.Rigidbody.mass);
        else enemy.Rigidbody.AddForce(Vector2.right * GetDirection() * -force * enemy.Rigidbody.mass);
    }

    public void DisableMovement()
    {
        disabled = true;
    }

    public void EnableMovement()
    {
        disabled = false;
    }

}
