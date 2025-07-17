using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Colected");

        }
    }
    
}
