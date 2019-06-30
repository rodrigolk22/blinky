using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Função para dar efeito de piscar no texto de iniciar jogo
public class BlinkText : MonoBehaviour {

    private Text text;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Sin(Time.time * 4));
    }
}
