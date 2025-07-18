using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float verticalForce = 3f;
    [SerializeField] private float horizontalForce = 3f;
    [SerializeField] private float initDelay = 3f;
    [SerializeField] private float onTime = 3f;
    [SerializeField] private float offTime = 3f;
    [SerializeField] private ParticleSystem airParticles;

    private Animator animator;
    private PlayerController player;
    private bool on;


    void Awake()
    {
      animator = GetComponent<Animator>();
    }

     void Start()
    {
      
        StartCoroutine(nameof(WorkingLoop));
    }

    void Update()
    {
        if (player != null && on)
        {
            Vector2 force = new Vector2(transform.up.x * horizontalForce, transform.up.y * verticalForce);
            player.AddExternalForce(force * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
   
        }
    }

     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }


    // ===== Functions Private =====

    private IEnumerator WorkingLoop()
    {
        yield return new WaitForSeconds(initDelay);

        while (gameObject.activeInHierarchy) {
            on = true;
            animator.SetBool("On", true);
            airParticles.Play();
            yield return new WaitForSeconds(onTime);
            on = false;
            airParticles.Stop();
            animator.SetBool("On", false);
            yield return new WaitForSeconds(offTime);
        }
    }
}
