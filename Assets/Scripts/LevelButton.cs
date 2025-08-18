using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour

{
    [SerializeField] private TextMeshProUGUI levelNumberText;

    private int levelNumber;

    public void SetLevel(int level)
    {
        levelNumber = level;

        levelNumberText.text = levelNumber.ToString();
    }

    public void StartLevel()
    {
        GameManager.Instance.LoadLevel(levelNumber); // Llama al método LoadLevel del GameManager para iniciar el nivel
    }

    public void SetLocked(bool loked)
    {
        GetComponent<Button>().interactable = !loked;
    }

}
