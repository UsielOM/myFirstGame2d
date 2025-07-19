using UnityEngine;

public class Spikes : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           collision.GetComponent<PlayerController>().Kill();
        }
    }


}
