using System.Collections;
using UnityEngine;

public class RinoEnemy : Enemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;

    //Private

    private bool movingRight = true;
    private float currentSpeed;
    private bool stopped;



    void Start()
    {
        currentSpeed = speed;
    }

     void Update()
    {
        if (!isDead)
        {
            if (!stopped)
            {
                print("RinoEnemy Update called");
                bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer);// si hay muros
                animator.SetBool("Run", true);
                if (hittingWall && !bossDamge)
                {
                    animator.SetBool("Run", false);
                    animator.SetBool("Ideal", true);
                    StartCoroutine(nameof(Flip));
                } else if (bossDamge)
                {
                    StartCoroutine(nameof(TakeDamageBoss));
                }

            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            float direction = movingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y); // movimiento
        
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
    }

    private IEnumerator Flip()
    {
        currentSpeed = 0;
        stopped = true;
        yield return new WaitForSeconds(stopDuration);
        animator.SetBool("Ideal", false);
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        currentSpeed = speed;
        stopped = false;
    }

    private IEnumerator TakeDamageBoss()
    { 
        currentSpeed = 0;
        stopped =  true;
        bossDamge = false;
        yield return new WaitForSeconds(stopDuration);
        animator.SetBool($"Dead", false);
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        currentSpeed = speed;
        stopped = false;
    }

}

