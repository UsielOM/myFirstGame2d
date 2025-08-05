using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float resetDelay = 1.5f; // Retraso antes de reiniciar el nivel
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null) {
            Instance = this; // Esto quiere decir que se guardara el GameManager
            DontDestroyOnLoad(gameObject); // Mantener este GameManager a través de las escenas
        }  
        else Destroy(Instance); // Si ya existe, destruir este GameManager para evitar duplicados
    }


    public void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // Obtener el índice del nivel actual
        int nextLevel = currentLevel + 1; // Incrementar el índice para cargar el siguiente nivel
        if (nextLevel < SceneManager.sceneCountInBuildSettings) // Verificar que el siguiente nivel existe
        {
            SceneManager.LoadScene(nextLevel); // Cargar el siguiente nivel
        }
        else
        {
            SceneManager.LoadScene(0); // Si no hay más niveles, reiniciar al primer nivel
        }

    }

    public void ResetLevel()
    {
        StartCoroutine(nameof(ResetLevelDelayed)); // Iniciar la corrutina para reiniciar el nivel después de un retraso
    }

    private IEnumerator ResetLevelDelayed()
    {
        yield return new WaitForSeconds(resetDelay); // Esperar 1 segundo antes de reiniciar el nivel
        int currentLevel = SceneManager.GetActiveScene().buildIndex; // Obtener el índice del nivel actual
        SceneManager.LoadScene(currentLevel); // Reiniciar el nivel actual
    }
}
