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
}
