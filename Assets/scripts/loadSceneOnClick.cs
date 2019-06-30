using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Função auxiliar que gerencia a chamada das telas do jogo
public class loadSceneOnClick : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
