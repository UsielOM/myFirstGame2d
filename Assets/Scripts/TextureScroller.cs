using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0, 0.05f);

    private Material material; // material to scroll the texture on
    private Vector2 offset; // desplasamiento de la textura
    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material; // Obtenemos el material del SpriteRenderer

    }

    // Update is called once per frame
    void Update()
    {
        offset += scrollSpeed * Time.deltaTime; // Actualizamos el desplazamiento de la textura
        material.mainTextureOffset = offset; // Aplicamos el desplazamiento al material
    }
}
