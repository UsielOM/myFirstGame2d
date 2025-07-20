using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //--Ser
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Private

    private Rigidbody2D rb;
    private bool movingRight = true;




     void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool noGrounded = !Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);// si hay suelo
        bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer);// si hay muros

        if (noGrounded || hittingWall)
        {
            Flip();
        }
    }
     void FixedUpdate()
    {
        float direction = movingRight ? 1f: -1f;
        rb.linearVelocity = new Vector2 (direction * speed, rb.linearVelocity.y); //moovimiento  
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * 1);
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
    }

    private void Flip()
    {
        movingRight = !movingRight;// vale lo contrario 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }
}
