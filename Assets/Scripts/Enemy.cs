using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;


    protected bool isDead = false;
    protected bool bossDamge = false;


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

            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController.VerticalVelocity < 0)
            {
                if(gameObject.CompareTag("Boss")) {
                    animator.SetBool("Dead", true);
                    bossDamge = true;
                    playerController.Impulse(Vector2.up, 7, true);
                    LevelController.Instance.BossDefeated();
                } else
                {
                    playerController.Impulse(Vector2.up, 7, true);
                    Die();
                }
            }
            else  playerController.Kill();
        }
    }

    private void Die() {

        col.enabled = false;
        rb.AddForce(Vector2.up * impulseForce);
        animator.SetBool("Dead", true);
        isDead = true;
    }


}
