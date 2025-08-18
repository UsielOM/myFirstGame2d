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
        }
    }
}
