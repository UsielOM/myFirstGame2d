
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar el siguiente nivel

public class LevelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitsCounterLavel;

    private int totalFruits;
    private int collectedFruits;

    public static LevelController Instance;

     void Awake()
    {
        if (Instance == null) Instance = this;   //Esto quiere decir que se guardara el level controler 

        else Destroy(Instance);
        
    }

    void Start()
    {
        totalFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None).Length;// nueva version

        fruitsCounterLavel.text = $"{collectedFruits} / {totalFruits}";
    }

    public void AddCollectedFruit()
    {
        collectedFruits++;
        fruitsCounterLavel.text = $"{collectedFruits} / {totalFruits}";

        if (collectedFruits >= totalFruits)
        {
            LoadNextLevel(); // Cargar el siguiente nivel si se han recolectado todas las frutas
        }
    }

    private void LoadNextLevel()
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

}
