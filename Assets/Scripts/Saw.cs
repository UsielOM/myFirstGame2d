using System.Collections;
using UnityEngine;

public class Saw : MonoBehaviour
{

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private Vector3 targetPosition;

    void Start()
    {
        transform.position = pointA.position;
        targetPosition = pointB.position;
       

        StartCoroutine(nameof(WorkingLoop));
    }


    private IEnumerator WorkingLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);//calculamos el movimiento de la distancia hasta doden tiene que llegar
                yield return null;// Esto va esperar a dibujar el frame
            }
            transform.position = targetPosition;
            yield return new WaitForSeconds(waitTime);
            targetPosition = transform.position == pointA.position ? pointB.position : pointA.position;
        }
    }
}
