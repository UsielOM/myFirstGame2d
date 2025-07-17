using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter2D(Collider2D collision) // Parametro de entrada
    {
        if(collision.gameObject.CompareTag("Player")) {
            print("Ha entrado el jugador");
        }
    }

}
