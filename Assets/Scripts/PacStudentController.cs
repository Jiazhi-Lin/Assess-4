using UnityEngine;
using UnityEngine.UI;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float currentMoveSpeed;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Vector3 lastInput;
    private Vector3 currentInput;
    public LayerMask obstacleLayer;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip wallCollisionAudioClip;
    public AudioClip moveAudioClip;
    public GameObject collisionParticlePrefab;
    public Text scoreText;
    private int score = 0;

    private const float TELEPORT_THRESHOLD = 0.1f;
    private const float COLLISION_THRESHOLD = 0.1f;
    private const float TELEPORT_COOLDOWN = 0.5f;

    private Vector3 teleportPoint1 = new Vector3(18.5f, -8.5f, 0);
    private Vector3 teleportPoint2 = new Vector3(-10f, -8.5f, 0);
    private float teleportCooldownTimer = 0f;
    private Vector3 previousPosition;
    private bool hasCollidedWithWall = false;

    void Start()
    {
        targetPosition = transform.position;
        previousPosition = transform.position;
        lastInput = Vector3.zero;
        currentInput = Vector3.zero;
        currentMoveSpeed = moveSpeed;
        UpdateScoreUI();
    }

    void Update()
    {
        HandleInput();

        if (teleportCooldownTimer > 0)
        {
            teleportCooldownTimer -= Time.deltaTime;
        }

        if (!isMoving) AttemptMove();

        if (isMoving) MoveTowardsTarget();
        else StopMovementEffects();

        CheckForTeleport();

        if (!isMoving)
        {
            hasCollidedWithWall = false;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            currentMoveSpeed = moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.W)) lastInput = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = Vector3.right;
    }

    void AttemptMove()
    {
        Vector3 nextPosition = transform.position + lastInput;

        if (IsWalkable(nextPosition))
        {
            currentInput = lastInput;
            previousPosition = transform.position;
            MoveToGridPosition(nextPosition);
        }
        else
        {
            HandleWallCollision(nextPosition);
        }
    }

    void MoveToGridPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        isMoving = true;
        PlayMovementEffects();
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < COLLISION_THRESHOLD)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    void HandleWallCollision(Vector3 nextPosition)
    {
        if (!hasCollidedWithWall)
        {
            audioSource.PlayOneShot(wallCollisionAudioClip);
            hasCollidedWithWall = true;
        }

        Instantiate(collisionParticlePrefab, nextPosition, Quaternion.identity);
        transform.position = previousPosition;

        currentMoveSpeed = 0;
        isMoving = false;
        targetPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pellet"))
        {
            Destroy(other.gameObject);
            score += 10;
            UpdateScoreUI();
        }
        else if (other.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            score += 100;
            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void PlayMovementEffects()
    {
        animator.SetBool("isMoving", true);
        PlayMovementAudio();
    }

    void StopMovementEffects()
    {
        StopMovementAudio();
        animator.SetBool("isMoving", false);
    }

    void PlayMovementAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = moveAudioClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void StopMovementAudio()
    {
        if (audioSource.isPlaying) audioSource.Stop();
    }

    bool IsWalkable(Vector3 position)
    {
        return Physics2D.Raycast(position, Vector2.zero, 0f, obstacleLayer).collider == null;
    }

    void CheckForTeleport()
    {
        if (teleportCooldownTimer <= 0)
        {
            if (Vector3.Distance(transform.position, teleportPoint1) < TELEPORT_THRESHOLD)
            {
                Teleport(teleportPoint2);
            }
            else if (Vector3.Distance(transform.position, teleportPoint2) < TELEPORT_THRESHOLD)
            {
                Teleport(teleportPoint1);
            }
        }
    }

    void Teleport(Vector3 newPosition)
    {
        transform.position = newPosition;
        targetPosition = newPosition;
        isMoving = false;
        teleportCooldownTimer = TELEPORT_COOLDOWN;
    }

   
}
