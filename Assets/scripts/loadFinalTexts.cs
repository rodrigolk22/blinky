using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Função que exibe os créditos finais quando o jogo acaba
public class loadFinalTexts : MonoBehaviour {

    public GameObject finalScore;
	void Start () {
		finalScore.GetComponent<Text>().text = Data.totalPoints.ToString();
    }
    private void FixedUpdate()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Credits");
            Data.lifeCounter = 3;
            Data.totalPoints = 0;
        }
    }

}
