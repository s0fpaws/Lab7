using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float floatSpeed = 0.5f; // Speed of the floating motion
    public float floatHeight = 0.2f; // Maximum height difference

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position; // Store the initial position
    }

    private void Update()
    {
        // Make the object float up and down smoothly
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Item collected!");
        }
    }
}