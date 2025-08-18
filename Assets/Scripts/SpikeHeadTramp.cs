using UnityEngine;

public class SpikeHeadTramp : MonoBehaviour
{

    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform pointA;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float sizeRycast = 15f;


    // Private
    private Vector3 targetPosition;
    private bool isFeeling = false;
    private Rigidbody2D rb;

     void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool hittingPlayer = Physics2D.Raycast(playerCheck.position, Vector2.down, sizeRycast, playerLayer);
        if (hittingPlayer) {
            isFeeling = true;
            Feeling();
        }
        bool hittingGround = Physics2D.Raycast(playerCheck.position, Vector2.down, 3, groundLayer);
        if (hittingGround) isFeeling = false;
    }


    private void Feeling()
    {
      if(isFeeling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -1 * speed);
        }
      
    }

    private void OnDrawGizmos()
    {
        if (!isFeeling)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCheck.position, Vector2.down * sizeRycast);

        }
       
    }
}
