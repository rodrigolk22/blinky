using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killHero : MonoBehaviour {

    public GameObject magician;

    /* Parte do inimigo que mata o herói, está separada pois 
     * somente a parte da frente do inimigo que colide */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (magician.GetComponent<MagicianBehaviour>().died == false)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerControl>().died = true;
            }
        }
    }
}
