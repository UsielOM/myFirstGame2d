using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float initDelay = 3f;
    [SerializeField] private float onTime = 3f;
    [SerializeField] private float offTime = 3f;

    // Private 
    private Animator animator;
    private PlayerController player;
    private bool isOn;

     void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(nameof(WorkingLoop));
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && isOn)
        {
            player.Kill(); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
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

    private IEnumerator WorkingLoop() 
    { 
        yield return new WaitForSeconds(initDelay);
        while (true)
        {
            animator.SetBool("On", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("Fire", true);
            isOn = true;
            yield return new WaitForSeconds(onTime);
            animator.SetBool("On", false);
            animator.SetBool("Fire", false);
            isOn = false;
            yield return new WaitForSeconds(offTime);
        }

    }
}
