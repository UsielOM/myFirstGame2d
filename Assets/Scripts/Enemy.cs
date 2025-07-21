using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;


    protected bool isDead = false;  


    private float impulseForce = 5f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
           PlayerController playerController =  collision.gameObject.GetComponent<PlayerController>();
            if (playerController.transform.position.y < transform.position.y + transform.localScale.y / 2)
            {
                playerController.Kill();
            }
            else
            {
                playerController.Impulse(Vector2.up, 7, true);
                Die();
            }
        }
    }

    private void Die() {

        col.enabled = false;
        rb.AddForce(Vector2.up * impulseForce);
        animator.SetBool("Dead", true);
        isDead = true;
    }
}
