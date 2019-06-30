using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        //Calcula e armazena o valor de deslocamento, obtendo a distância entre a posição do player e a posição da câmera.
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, transform.position.z);
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // Define a posição da transformação da câmera para ser a mesma que a do jogador, mas compensada pela distância de deslocamento calculada.
        transform.position = player.transform.position + offset;
    }
}
