using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void PLay()
    {
        GameManager.Instance.LoadNextLevel(); // Llama al m�todo LoadNextLevel del GameManager para iniciar el juego
    }
    public void ExitGame()
    {
       Application.Quit(); // Cierra la aplicaci�n
    }
}
