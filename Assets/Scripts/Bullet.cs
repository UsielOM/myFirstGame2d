using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private BulletPiece[] bulletPiecesPrefabs; // se convierte en array por que tensmo dos bullet pice


    private Rigidbody2D rb;
    private Vector2 bulletSpeed;

     void FixedUpdate()
    {
        rb.linearVelocity = bulletSpeed;

    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Kill();
        }
        //Vector 3 a Vector 2 castear 
        Vector2 spawnPoint = (Vector2)transform.position - bulletSpeed.normalized * 0.25f;
        foreach (BulletPiece bulletPiecePrefabItem in bulletPiecesPrefabs)
        {
            Instantiate(bulletPiecePrefabItem, spawnPoint , Quaternion.identity).DestroyerDelayed();
        }

        Destroy(gameObject);

    }

  

    public void Init(float bulletSpeed, Vector2 dir,bool isHor) //metodos para instanciar 
    {
        //velocidad y direccion
        rb = GetComponent<Rigidbody2D>();
        this.bulletSpeed = bulletSpeed *  dir; 
        float horScale = isHor ? Mathf.Abs(transform.localScale.x) * dir.x : 
            Mathf.Abs(transform.localScale.y) * dir.y;// orientacion de la bala
        transform.localScale = new Vector2(horScale, transform.localScale.y);
    }

}
