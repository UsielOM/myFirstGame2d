using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;
    [SerializeField] private AudioClip deadSound;


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
            float positionVerticalEnemy = rb.position.y + 1f; 
            if (playerController.VerticalVelocity > positionVerticalEnemy)
            {
                if (gameObject.CompareTag("Boss"))
                {
                    animator.SetBool("Dead", true);
                    bossDamge = true;
                    playerController.Impulse(Vector2.up, 7, true);
                    LevelController.Instance.BossDefeated();
                }
                else
                {
                    playerController.Impulse(Vector2.up, 7, true);
                    Die();
                }
            }
            else {
                playerController.Kill();
            } 
        }
    }

    private void Die() {

        col.enabled = false;
        rb.AddForce(Vector2.up * impulseForce);
        animator.SetBool("Dead", true);
        isDead = true;
        GameManager.Instance.PlaySound(deadSound);
    }


}
