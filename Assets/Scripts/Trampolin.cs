using UnityEngine;

public class Trampolin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float force = 10f;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            anim.SetTrigger("jumped");
            collision.GetComponent<PlayerController>().Inpulse(Vector2.up, force);

        }
    }
}