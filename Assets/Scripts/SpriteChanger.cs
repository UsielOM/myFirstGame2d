using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites; // arreglod sprites a cambiar

     void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)]; // Cambiamos el sprite al azar al iniciar
    }
}
