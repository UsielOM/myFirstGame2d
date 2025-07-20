using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           collision.GetComponent<PlayerController>().Kill();
        }
    }


}
