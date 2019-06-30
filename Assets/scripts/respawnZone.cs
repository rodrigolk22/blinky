using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnZone : MonoBehaviour {

    public GameObject enemy;
    public GameObject respawnPosition;
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Função instancia um inimigo, que é criado na mesma posição da zona de reaparecimento "respawnZone"
    public void respawnEnemy()
    {
        anim.SetTrigger("open");
        Instantiate(enemy, respawnPosition.gameObject.transform.position, respawnPosition.gameObject.transform.rotation);
        StartCoroutine("closeDoor");
    }
    // Anima a porta fechando
    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(1);
        anim.ResetTrigger("open");
    }
}
