using UnityEngine;

public class BulletPiece : MonoBehaviour
{

    public void DestroyerDelayed ()
    {
       GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

}
