using System.Collections;
using UnityEngine;

public class FatBirdBoss : Enemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private float flyForce = 4f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform topCheck;
    [SerializeField] private LayerMask groundLayer; // Layer for ground and walls
    [SerializeField] private LayerMask playerLayer;

    // Private
    private bool isMovingRight;
    private bool movingRight;
    private bool isMovingTop = true;
    private float currentSpeed;
    private bool isStopped;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (bossDamge) StartCoroutine(nameof(TakeDamageBoss));
            else if (!isStopped)
            {
              
                bool hittingTop = Physics2D.Raycast(topCheck.position, Vector2.up, 0.2f, groundLayer);
                if (hittingTop) isMovingRight = true;

                bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer); 
                if(hittingWall) StartCoroutine(nameof(Flip));

                bool hittingPlayer = Physics2D.Raycast(groundCheck.position, Vector2.down, 8f, playerLayer);
                if (hittingPlayer && isMovingRight) {

                    isMovingTop = false;
                    isMovingRight = false;
                    animator.SetBool($"Fall", true);
                }

                bool hittingGround = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
                if (hittingGround && !isMovingTop) StartCoroutine(nameof(FallIdle));

            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            FlyBoss();
            MovingBoss();
        }

    }


    private void FlyBoss()
    {
 
            float direction = isMovingTop ? 1f : -1f;
            float currentDirection = isMovingTop ? 3f : 10f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, direction * currentDirection);
        
    }

    private void MovingBoss()
    {
        if (isMovingRight)
        {
            float direction = movingRight ? -1f : 1f;
            rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
        if (isMovingTop) Gizmos.DrawRay(topCheck.position, Vector2.up * 0.2f);
        else Gizmos.DrawRay(groundCheck.position, Vector2.down * 0.2f);
        if (isMovingRight) {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(groundCheck.position, Vector2.down * 8f);
        }
    }

    private IEnumerator Flip()
    {
        currentSpeed = 0;
        isStopped = true;
        yield return new WaitForSeconds(stopDuration);
        movingRight = !movingRight;// vale lo contrario 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        currentSpeed = speed;
        isStopped = false;

    }

    private IEnumerator FallIdle()
    {
        animator.SetBool($"Fall", false);
        animator.SetTrigger($"Grounded");
        currentSpeed = 0;
        isStopped = true;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger($"Idel");
        isMovingTop = !isMovingTop;
        currentSpeed = speed;
        isStopped = false;
    }
    private IEnumerator TakeDamageBoss()
    {
        currentSpeed = 0;
        isStopped = true;
        bossDamge = false;
        yield return new WaitForSeconds(stopDuration);
        animator.SetBool($"Dead", false);
        if(isMovingTop) animator.SetTrigger($"Idel");
        else animator.SetTrigger($"Fall");
        currentSpeed = speed;
        isStopped = false;
    }

}
