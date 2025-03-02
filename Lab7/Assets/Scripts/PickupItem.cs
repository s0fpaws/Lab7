using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // When the player collides with the pickup item
    private void OnTriggerEnter(Collider other)
    {
        // Debug log to check if collision is detected
        Debug.Log("Collided with: " + other.gameObject.name);

        // Check if the object colliding with the pickup item is the player
        if (other.CompareTag("Player"))
        {
            // Destroy the pickup item when it touches the player
            Destroy(gameObject);
            Debug.Log("Item collected!");
        }
    }
}
