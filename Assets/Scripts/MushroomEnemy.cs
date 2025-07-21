using System.Collections;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //--Ser
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Private

    private bool movingRight = true;
    private float currentSpeed;
    private bool stopped;

     void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!stopped)
            {
                bool noGrounded = !Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);// si hay suelo
                bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer);// si hay muros
                animator.SetBool("Run", true);
                if (noGrounded || hittingWall)
                {
                    animator.SetBool("Run", false);
                    StartCoroutine(nameof(Flip));
                }
            }
        }
    }
     void FixedUpdate()
    {
       
       if (!isDead)
        {
            float direction = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y); //moovimiento
        }
                                                  
    }

     void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * 1);
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
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
