using TMPro;
using UnityEngine;

public class InterfaceTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTest;//Una variable que puede conter texto 

    void Start()
    {
        textTest.text = "Hello World";
    }


    void Update()
    {
        
    }


    public void SayHello()
    {
        print("Hello World");
    }

    public void ActivateSomeThing(bool isOn)
    {
        print($"El estado del toggel es: {isOn}");
    }

    public void ChangedVolume(float sliderValue)
    {
        print($"El volumen a cambhiado a : {sliderValue}");
    }

    public void ChangedName(string text)
    {
        print($"El texto ha cambiado a: {text}");
    }

    public void OnEndEdit(string text)
    {
        print($"El texto final es: {text}");
    }
}
