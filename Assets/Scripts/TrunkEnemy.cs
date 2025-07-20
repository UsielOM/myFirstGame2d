using System.Collections;
using UnityEngine;

public class TrunkEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //--Ser
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private float detectPlayerDistance = 5f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    //Private

    private Rigidbody2D rb;
    private bool movingRight = true;
    private float currentSpeed;
    private bool stopped;
    private bool shooting;
    private Animator animator;




    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped && !shooting)
        {
            bool noGrounded = !Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);// si hay suelo
            bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer);// si hay muros
            if (noGrounded || hittingWall)
            {
                StartCoroutine(nameof(Flip));
            }

        }

        Vector2 dir = movingRight ? Vector2.right : Vector2.left;// creamos un detector de usaurios para disparar
        bool detectedPlaying = Physics2D.Raycast(wallCheck.position, dir, detectPlayerDistance, playerLayer);
        if (detectedPlaying && !shooting) { 
            shooting= true;
            currentSpeed = 0;
            animator.SetBool("Shoot", true);
        } else if (!detectedPlaying && shooting)
        {
            shooting = false;
            currentSpeed = speed;
            animator.SetBool("Shoot", false);
        }
        animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));
    }
    void FixedUpdate()
    {

        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y); //moovimiento

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * 1);
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
        Gizmos.color = Color.yellow;
        Vector2 dir = movingRight ? Vector2.right : Vector2.left;// creamos un detector de usaurios para disparar
        Gizmos.DrawRay(wallCheck.position, dir * detectPlayerDistance);
    }

    private IEnumerator Flip()
    {
        currentSpeed = 0;
        stopped = true;
        yield return new WaitForSeconds(stopDuration);
        movingRight = !movingRight;// vale lo contrario 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        currentSpeed = speed;
        stopped = false;

    }
}
