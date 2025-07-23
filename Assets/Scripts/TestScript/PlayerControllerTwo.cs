using UnityEngine;

public class PlayerControllerTwo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement")]
    [SerializeField] private float speed = 5f; // Speed of the player

    // ==== Private Variables ====
    private float xInput = 0f; // Horizontal input
    private Rigidbody2D rb; // Reference to the Rigidbody2D component


     void Awake()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

     void FixedUpdate()
    {
        // Apply movement based on input
        // The linearVelocity is set to the horizontal input multiplied by speed, and the vertical velocity remains unchanged}
        if (rb == null) return;
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        DetectedInput();
    }


    private void DetectedInput()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (left/right arrow keys or A/D keys)    

    }
}
