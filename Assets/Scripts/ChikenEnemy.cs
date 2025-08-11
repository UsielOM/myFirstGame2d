using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChikenEnemy : Enemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDuration = 0.75f;
    [SerializeField] private Transform playerHorCheck;
    [SerializeField] private Transform playerVerCheck;
    [SerializeField] private LayerMask playerLayer;

    // Private

    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    private float currentSpeed;
    private bool isStopped = false;

    private Vector2 moveDirection => !isMovingLeft ? Vector2.right : Vector2.left; // getter



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if(!isStopped)
            {
                bool hittingPlayerHor = Physics2D.Raycast(playerHorCheck.position, moveDirection, 15f, playerLayer);
                if (hittingPlayerHor) {
                    isMovingRight = !isMovingRight;
                    animator.SetBool("Run", true);
                }
                else
                {
                    isMovingRight = false;
                    animator.SetBool("Run", false);
                }
                bool hittingPlayerVer = Physics2D.Raycast(playerVerCheck.position, Vector2.up, 3f, playerLayer);
                if (hittingPlayerVer)StartCoroutine(nameof(Flip));

            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Moving();
        }
    }

    void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerHorCheck.position, moveDirection * 15);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerVerCheck.position, Vector2.up * 3f);
    }

    private IEnumerator Flip() {
        animator.SetBool("Run", false);
        currentSpeed = 0;
        isStopped = true;
        yield return new WaitForSeconds(stopDuration);
        isMovingLeft = !isMovingLeft;// vale lo contrario 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        currentSpeed = speed;
        isStopped = false;
    }

    private void Moving()
    {
        if(isMovingRight)
        {
            float direction = isMovingLeft ? -1f : 1f;
            rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
        }
    }
}