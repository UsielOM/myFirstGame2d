using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform levelButtonsContainer; // Contenedor de los botones de nivel
    [SerializeField] private LevelButton levelButtonPrefab; // Prefab del botón de nivel

    void Start()
    {
        SpawnLevelButtons();

    }
    public void PLay()
    {
        GameManager.Instance.LoadNextLevel(); // Llama al método LoadNextLevel del GameManager para iniciar el juego
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("HigherLevel"); // Elimina el progreso guardado del jugador
        SceneManager.LoadScene(0); // Reinicia al primer nivel
    }
    public void ExitGame()
    {
       Application.Quit(); // Cierra la aplicación
    }

    private void SpawnLevelButtons()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings;

        for (int i = 1; i < totalLevels; i++)
        {
            LevelButton currentLevelButton = Instantiate(levelButtonPrefab, levelButtonsContainer);
            currentLevelButton.SetLevel(i);
            bool isLocked = i > PlayerPrefs.GetInt("HigherLevel") + 1; // Verifica si el nivel está bloqueado
            currentLevelButton.SetLocked(isLocked); // Bloquea el botón si el nivel es mayor al nivel más alto alcanzado
        }
    }
}
