using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeeEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float speed = 2f; // Speed of the bee enemy
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private float detectPlayerDistance = 5f; // Distance to detect the player
    [SerializeField] private Transform wallCheck; // Transform to check for walls
    [SerializeField] private Transform playerCheck; // Transform to check for the player
    [SerializeField] private LayerMask groundLayer; // Transform to check for the player
    [SerializeField] private LayerMask playerLayer; // Transform to check for the player
    [SerializeField] private Bullet bulletPrifab;
    [SerializeField] private float bulletSpeed;

    private bool movingRight = true;
    private float currentSpeed;
    private bool stopped;
    private bool shooting;

    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!stopped && !shooting)
            {
              
                bool hittingWall = Physics2D.Raycast(wallCheck.position, transform.right, 0.2f, groundLayer);// si hay muros
                if ( hittingWall)
                {
                    StartCoroutine(nameof(Flip));
                }
            }


            bool detectedPlaying = Physics2D.Raycast(playerCheck.position, Vector2.down, detectPlayerDistance, playerLayer);
            if (detectedPlaying && !shooting)
            {
                shooting = true;
                currentSpeed = 0;
                animator.SetBool("isShoot", true);
            }
            else if (!detectedPlaying && shooting)
            {
                shooting = false;
                currentSpeed = speed;
                animator.SetBool("isShoot", false);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheck.position, transform.right * 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCheck.position, Vector2.down * 2.5f);
    }
    private void Shoot()
    {
        //Instantiate(bulletPrifab,wallCheck.position, Quaternion.identity);//estamso creando uan instancia de la bala
        Instantiate(bulletPrifab, playerCheck.position, Quaternion.identity).Init(bulletSpeed, Vector2.down, false);
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
