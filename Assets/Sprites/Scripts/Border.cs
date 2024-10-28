using UnityEngine;
using UnityEngine.UI;

public class BorderAnimator : MonoBehaviour
{
    public float speed = 1.0f;
    public RectTransform borderTransform;
    public Vector3[] positions; // Define positions for the animated dots

    private int currentPosIndex = 0;

    void Start()
    {
        // Initialize the positions (you may adjust these based on your design)
        positions = new Vector3[]
        {
            new Vector3(-750, 50, 0), // Top Left
            new Vector3(750, 50, 0),  // Top Right
            new Vector3(750, -850, 0), // Bottom Right
            new Vector3(-750, -850, 0)  // Bottom Left
        };

        borderTransform.localPosition = positions[currentPosIndex];
    }

    void Update()
    {
        // Move the border to the next position
        borderTransform.localPosition = Vector3.MoveTowards(
            borderTransform.localPosition,
            positions[currentPosIndex],
            speed * Time.deltaTime
        );

        // Check if we reached the current position
        if (Vector3.Distance(borderTransform.localPosition, positions[currentPosIndex]) < 0.1f)
        {
            currentPosIndex = (currentPosIndex + 1) % positions.Length; // Loop through positions
        }
    }
}
