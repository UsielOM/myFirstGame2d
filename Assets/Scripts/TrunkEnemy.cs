using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class TrunkEnemy : Enemy
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
    [SerializeField] private Bullet bulletPrifab;
    [SerializeField] private float bulletSpeed;


    //Private
    private bool movingRight = true;
    private float currentSpeed;
    private bool stopped;
    private bool shooting;

    private Vector2 moveDirection => movingRight ? Vector2.right : Vector2.left; // getter

    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {

        if(!isDead)
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


            bool detectedPlaying = Physics2D.Raycast(wallCheck.position, moveDirection, detectPlayerDistance, playerLayer);
            if (detectedPlaying && !shooting)
            {
                shooting = true;
                currentSpeed = 0;
                animator.SetBool("Shoot", true);
            }
            else if (!detectedPlaying && shooting)
            {
                shooting = false;
                currentSpeed = speed;
                animator.SetBool("Shoot", false);
            }
            animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(wallCheck.position, moveDirection * detectPlayerDistance);
    }

    private void Shoot()
    {
        //Instantiate(bulletPrifab,wallCheck.position, Quaternion.identity);//estamso creando uan instancia de la bala
        Instantiate(bulletPrifab, wallCheck.position, Quaternion.identity).Init(bulletSpeed, moveDirection, true);
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
