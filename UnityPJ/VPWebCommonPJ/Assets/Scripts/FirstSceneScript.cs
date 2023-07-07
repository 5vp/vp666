using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneScript : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Scenes/CommonSTScene");
    }

    void Update()
    {
        
    }
}
