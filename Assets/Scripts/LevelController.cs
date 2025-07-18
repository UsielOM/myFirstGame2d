using UnityEngine;

public class LevelController : MonoBehaviour
{
    
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
        print(totalFruits);
    }

    public void AddCollectedFruit()
    {
        collectedFruits++;
        print($"Frutas recojidas: {collectedFruits}");

        if (collectedFruits >= totalFruits) print("Nivel completado! :D"); 
    }

}
