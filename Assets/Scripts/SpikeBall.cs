using UnityEngine;

public class SpikeBall : MonoBehaviour
{

    [SerializeField] private float maxAngle = 45f; 
    [SerializeField] private float speed = 2f;

    private float time;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;
        //calcular el angulo para el movimiento

        float angle = Mathf.Sin(time) * maxAngle;// esta linea hace el calculo del seno la rotacion

        transform.rotation = Quaternion.Euler(0,0,angle); // angulos de euler  aqui estamos haciendo que rote el objeto en Eje Z

    }
}

