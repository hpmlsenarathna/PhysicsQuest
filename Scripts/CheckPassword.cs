using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckPassword : MonoBehaviour
{
    private InputField inputTextField;

    void Start()
    {
        inputTextField = GetComponent<InputField>();
    }

    public void CheckPasswordAndLoadScene()
    {
        switch (inputTextField.text)
        {
            case "60":
                SceneManager.LoadScene("Play");
                break;

            default:
                inputTextField.text = "";
                break;
        }
    }
}
