
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar el siguiente nivel

public class LevelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitsCounterLavel;
    [SerializeField] private TextMeshProUGUI healtBossCounterLevel;
    [SerializeField] private AudioClip coinSound; // Clip de audio para el sonido de reinicio
    [SerializeField] private bool isBossLevel; // Indica si el nivel es un nivel de jefe

    private int totalFruits;
    private int collectedFruits;
    private int healthBoss = 3;
    [SerializeField]

    public static LevelController Instance;

     void Awake()
    {
        if (Instance == null) Instance = this;   //Esto quiere decir que se guardara el level controler 

        else Destroy(Instance);
        
    }

    void Start()
    {
        if(!isBossLevel)
        {
            totalFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None).Length;// nueva version

            fruitsCounterLavel.text = $"{collectedFruits} / {totalFruits}";
        } else
        {
            healtBossCounterLevel.text = $"{healthBoss}";
        }
        
    }

    public void AddCollectedFruit()
    {
        collectedFruits++;
        fruitsCounterLavel.text = $"{collectedFruits} / {totalFruits}";
        GameManager.Instance.PlaySound(coinSound, 0.5f); // Reproducir el sonido de moneda al recolectar una fruta
        if (collectedFruits >= totalFruits)
        {
            GameManager.Instance.LoadNextLevel(); // Cargar el siguiente nivel cuando se hayan recolectado todas las frutas desde el GameManager
        }
    }


    public void BossDefeated()
    {
       healthBoss -= 1; // Reducir la salud del jefe al ser derrotado
        healtBossCounterLevel.text = $"{healthBoss}";
        if (healthBoss <= 0)
        {
            GameManager.Instance.LoadNextLevel(); // Cargar el siguiente nivel cuando el jefe sea derrotado
        }
    }


}
