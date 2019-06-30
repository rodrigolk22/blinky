using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anyKey : MonoBehaviour {

    public int sceneIndex;
    public string sceneName;
    
    // Verifica se um tecla foi ativada e caminha para a próxima tela (scene)
    void FixedUpdate () {
        if (Input.anyKey)
        {
            if (!String.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }else if (sceneIndex > 0)
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }
}
