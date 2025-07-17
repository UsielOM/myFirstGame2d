using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D collision) // Parametro de entrada
    {
        //cuando detecte que algo entre al trigger puede ejecutar este codigo

        //print(collision.gameObject.name);// estamos obteniendo lo que entro en el trigger

    }

     void OnTriggerExit2D(Collider2D collision)// cuando algo sale del trigger
    {
       /// print(collision.gameObject.name); // lo que sale del colicionador
    }

      void OnTriggerStay2D(Collider2D collision)// Aqui genera un bucle constante mientras este dentro del trigger el gameObject  solo si se mueve
    {
        //print(collision.gameObject.name);
    }

}
