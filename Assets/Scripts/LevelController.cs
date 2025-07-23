
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
          GameManager.Instance.LoadNextLevel(); // Cargar el siguiente nivel cuando se hayan recolectado todas las frutas desde el GameManager
        }
    }

 

}
