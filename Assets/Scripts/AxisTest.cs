using UnityEngine;

public class AxisTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");//Me devuelve el valor de cualquier tipo de mando que tenga configurado el eje X solo que lo da interpolado 
        float y = Input.GetAxisRaw("Vertical");// Me devuelve el valor de cualquier tipo de mando que tenga configurado el eje Y solo que lo crudo directo
        print(x);
        print(y);
    }

}
