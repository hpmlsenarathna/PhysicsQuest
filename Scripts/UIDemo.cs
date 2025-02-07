using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDemo : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField userinput;

    public void ButtonEnter()
    {
        output.text = userinput.text;

     
    }

  
    public void ButtonExit()
    {
        // Quit the application
        Application.Quit();

        // Log a message to indicate exit action (useful during testing in the editor)
        Debug.Log("Exit button clicked. Application.Quit() will work in a built application.");
    
}

    public void LoadNextInBuild()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
