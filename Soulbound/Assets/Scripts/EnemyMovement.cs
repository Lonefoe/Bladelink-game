using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] private float moveSpeed = 25f;
    private float moveSpeedMultiplier = 1f;
    private float horizontalMove;
    private int direction = 1;

    public float MoveSpeedMultiplier
    {
        get
        {
            return moveSpeedMultiplier;
        }
        set
        {
            moveSpeedMultiplier = value;
        }
    }

    // Reference setup
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        enemy.Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    private void FixedUpdate()
    {
        horizontalMove = moveSpeed * moveSpeedMultiplier;

        if (direction == 1)
        {
            enemy.Controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
        }
        else if (direction == -1)
        {
            enemy.Controller.Move(-horizontalMove * Time.fixedDeltaTime, false, false);
        }
    }

    public void ChangeDirection(Vector2 targetPos)
    {
        if (transform.position.x <= targetPos.x)
        {
            direction = 1;
        }
        else direction = -1;
    }

    public int GetDirection()
    {
        return direction;
    }

}
