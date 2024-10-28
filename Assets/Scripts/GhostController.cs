using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Vector3 lastDirection;
    private List<Vector3> validDirections = new List<Vector3>();

    public enum GhostState { Dead, Walking, Scared, Recovering }
    public GhostState currentState;

    private Vector3 spawnAreaCenter = new Vector3(5, -8, 0); 
    private List<Vector3> spawnGaps; 

    void Start()
    {
        targetPosition = transform.position;
        currentState = GhostState.Dead;
        spawnGaps = new List<Vector3>(); 
    }

    void Update()
    {
        if (!isMoving)
        {
            DecideMovement();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    private void DecideMovement()
    {
        if (currentState == GhostState.Dead)
        {
            MoveToSpawnArea();
        }
        else if (currentState == GhostState.Walking || currentState == GhostState.Scared || currentState == GhostState.Recovering)
        {
            ExecuteGhostBehavior();
        }
    }

    private void MoveToSpawnArea()
    {
        Vector3 directionToSpawn = (spawnAreaCenter - transform.position).normalized;
        targetPosition = transform.position + directionToSpawn;
        isMoving = true;
    }

    private void ExecuteGhostBehavior()
    {
        validDirections = GetValidDirections();

        if (validDirections.Count == 0) return;

        Vector3 newDirection = Vector3.zero;

        switch (gameObject.name) 
        {
            case "Ghost1":
                newDirection = GetDirectionToKeepDistance();
                break;
            case "Ghost2":
                newDirection = GetDirectionToCloseDistance();
                break;
            case "Ghost3":
                newDirection = validDirections[Random.Range(0, validDirections.Count)];
                break;
            case "Ghost4":
                newDirection = GetClockwiseDirection();
                break;
        }

        targetPosition = transform.position + newDirection;
        lastDirection = newDirection;
        isMoving = true;
    }

    private List<Vector3> GetValidDirections()
    {
        List<Vector3> directions = new List<Vector3>
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        };

        List<Vector3> validDirections = new List<Vector3>();

        foreach (var dir in directions)
        {
            Vector3 nextPosition = transform.position + dir;
            if (IsWalkable(nextPosition) && dir != -lastDirection)
            {
                validDirections.Add(dir);
            }
        }

        return validDirections;
    }

    private Vector3 GetDirectionToKeepDistance()
    {
        Vector3 pacPosition = FindObjectOfType<PacStudentController>().transform.position;
        List<Vector3> validMoves = new List<Vector3>();

        foreach (var direction in validDirections)
        {
            Vector3 newPosition = transform.position + direction;
            if (Vector3.Distance(newPosition, pacPosition) >= Vector3.Distance(transform.position, pacPosition))
            {
                validMoves.Add(direction);
            }
        }

        return validMoves.Count > 0 ? validMoves[Random.Range(0, validMoves.Count)] : Vector3.zero;
    }

    private Vector3 GetDirectionToCloseDistance()
    {
        Vector3 pacPosition = FindObjectOfType<PacStudentController>().transform.position;
        List<Vector3> validMoves = new List<Vector3>();

        foreach (var direction in validDirections)
        {
            Vector3 newPosition = transform.position + direction;
            if (Vector3.Distance(newPosition, pacPosition) <= Vector3.Distance(transform.position, pacPosition))
            {
                validMoves.Add(direction);
            }
        }

        return validMoves.Count > 0 ? validMoves[Random.Range(0, validMoves.Count)] : Vector3.zero;
    }

    private Vector3 GetClockwiseDirection()
    {
       
        return Vector3.right; 
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    private bool IsWalkable(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        return hit.collider == null;
    }
}
