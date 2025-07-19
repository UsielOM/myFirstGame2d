using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private bool elevatorMode;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    private Animator animator;

    private Vector3 targetPosition;

    private Vector3 lastPosition;

    public Vector3 Velocity;

     void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        transform.position = pointA.position;
        targetPosition = elevatorMode ? pointA.position : pointB.position;
        lastPosition = transform.position;
        if(elevatorMode) animator.SetTrigger("Wood");
        StartCoroutine(nameof(WorkingLoop));
    }


     void LateUpdate() // Es un update uan vez por frame pero se ejecuta despues del update 
    {
        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        Velocity = new Vector3(Velocity.x, 0, 0);
        lastPosition = transform.position;
    }


     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().SetPlatform(this);//la paltaforma en la que esta aprado es uno mismo y se usa this
            if(elevatorMode) targetPosition = pointB.position;
         }
    }

     void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().SetPlatform(null);//la paltaforma en la que esta aprado es uno mismo y se usa this
            if (elevatorMode) targetPosition = pointA.position;
        }
    }


    private IEnumerator WorkingLoop()
    {
        while (gameObject.activeInHierarchy) {
            animator.SetBool("On", true);
            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);//calculamos el movimiento de la distancia hasta doden tiene que llegar
                yield return null;// Esto va esperar a dibujar el frame
            }
            transform.position = targetPosition;
            animator.SetBool("On", false);

            if (!elevatorMode)
            {
                yield return new WaitForSeconds(waitTime);
                targetPosition = transform.position == pointA.position ? pointB.position : pointA.position;
            }

            yield return null;
        }

        
    }
}
