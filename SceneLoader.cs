using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void SceneNavigator(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }


}
