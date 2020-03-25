using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyPathing : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] private Transform[] patrolPoints;
    PathType pathType;
    [SerializeField] private int startPatrolIndex = 1;
    [SerializeField] private float waitTime = 0;
    private bool goBack = true;
    private int currentPatrolIndex = 1;
    private int direction = 1;      // The equivalent of A and D the player uses for movement (1 = Right/D, -1 = Left/A)


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        currentPatrolIndex = startPatrolIndex;

        if (patrolPoints.Length > 2)
        {
            pathType = PathType.UseMiddlePoints;
        } else
        {
            pathType = PathType.IgnoreMiddlePoints;
        }

    }

    // Update is called once per frame
    void Update()
    {
        var enemyPos = new Vector2(transform.position.x, 0);
        var targetPos = new Vector2(patrolPoints[currentPatrolIndex].position.x, 0);

        if (Vector2.Distance(enemyPos, targetPos) < 0.2f)
        {
            ChangePatrolIndex();
            direction = -direction;

        }

        enemy.Movement.ChangeDirection(direction);

    }

    void ChangePatrolIndex()
    {
        switch (pathType)
        {
            case PathType.UseMiddlePoints:
              if (currentPatrolIndex == 0 && goBack == false)   // If our patrol index is first
              {
                    currentPatrolIndex = patrolPoints.Length - 1;  // Go to the last one
                    goBack = true;
              }
              else if (patrolPoints.Length - 1 <= currentPatrolIndex && goBack == true) // If it is the last and we should go back
              {
                    currentPatrolIndex--;                             // Go to the one that is lower
              }
              else if (patrolPoints.Length - 1 <= currentPatrolIndex && goBack == false) // If it is the last and we shouldn't go back
              {
                    currentPatrolIndex = 0;                                                // We ignore the middle points and go to the start
              } else if (patrolPoints.Length - 1 > currentPatrolIndex && currentPatrolIndex != 0) // If it is a middle point
              {
                 currentPatrolIndex++;                                                           // Make it go back to the end 
                  goBack = false;                                                                 // Reset the variable
               }

                break;

            case PathType.IgnoreMiddlePoints:
                if (currentPatrolIndex == 0)   // If our patrol index is first
                {
                    currentPatrolIndex = patrolPoints.Length - 1;  // Go to the last one

                }
                else if (patrolPoints.Length - 1 <= currentPatrolIndex) // If it is the last
                {
                    currentPatrolIndex = 0;                             // Go to the start
                }

                break;

        }

    }

}


public enum PathType
{
    UseMiddlePoints,
    IgnoreMiddlePoints
}
