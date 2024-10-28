using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab; // Cherry prefab
    public float spawnInterval = 10f; // Spawn interval in seconds
    public float moveSpeed = 2f; // Speed of cherry movement

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnCherry), 0f, spawnInterval);
    }

    void SpawnCherry()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveCherry(cherry));
    }

    Vector3 GetRandomSpawnPosition()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Randomly choose a side to spawn the cherry
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // Left
                return new Vector3(-cameraWidth - 1f, Random.Range(-cameraHeight / 2f, cameraHeight / 2f), 0f);
            case 1: // Right
                return new Vector3(cameraWidth + 1f, Random.Range(-cameraHeight / 2f, cameraHeight / 2f), 0f);
            case 2: // Top
                return new Vector3(Random.Range(-cameraWidth / 2f, cameraWidth / 2f), cameraHeight + 1f, 0f);
            case 3: // Bottom
                return new Vector3(Random.Range(-cameraWidth / 2f, cameraWidth / 2f), -cameraHeight - 1f, 0f);
            default:
                return Vector3.zero; // Fallback
        }
    }

    System.Collections.IEnumerator MoveCherry(GameObject cherry)
    {
        Vector3 targetPosition = new Vector3(-cherry.transform.position.x, cherry.transform.position.y, 0f); // Move in the opposite direction
        while (Vector3.Distance(cherry.transform.position, targetPosition) > 0.1f)
        {
            cherry.transform.position = Vector3.Lerp(cherry.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(cherry);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PacStudent"))
        {
            // Handle collision with PacStudent (e.g., give points, play sound)
            Destroy(gameObject);
        }
    }
}
