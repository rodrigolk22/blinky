using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Funções relacionadas à caixa "box"
public class box : MonoBehaviour {

    public bool grabbed;
    public Transform pivot;
    public bool throwed;
    public bool right;
    public float velocity = 0.05f;
    public GameObject itSelf;
    public BoxCollider2D gravityManager;
    public GameObject gameManager;

    void Start () {
        grabbed = false;
        throwed = false;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void Update()
    {
        /*Se caixa estiver no estado segurada "grabbed" ela assume a posição da mão 
         * do herói "pivotHero" e perde a gravidade
        */
        if(grabbed == true)
        {
            pivot = GameObject.FindGameObjectWithTag("pivotHero").GetComponent<Transform>();
            GetComponent<Rigidbody2D>().gravityScale = 0;
            gravityManager.enabled = false;
            transform.localPosition = pivot.transform.position;
        }
        /* Se a caixa for arremessada "throwed" ela deve se deslocar na direção
         que o herói estava olhando e assume as  forças da gravidade */
        if (throwed == true)
        {
            
            if (right == true)
            {
                transform.Translate(Vector3.right * velocity);
            }
            else
            {
                transform.Translate(Vector3.left * velocity);
            }
            GetComponent<Rigidbody2D>().gravityScale = 1;
            gravityManager.enabled = true;
        }
       
    }
    //Colisões
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ao colidir com a parede "Wall", a caixa perde a velocidade
        if (other.gameObject.CompareTag("Wall"))   
        {
            throwed = false;
            transform.Translate(Vector3.zero);
        }
        // Ao colidir com o solo "Ground", a caixa perde a velocidade
        if (other.gameObject.CompareTag("Ground"))
        {
            throwed = false;
            transform.Translate(Vector3.zero);
        }
        /* Ao colidir com o inimigo "Enemy", a caixa causa dano "hit" no
        inimigo e então ela é destruída*/
        if (other.gameObject.CompareTag("Enemy") && throwed == true)
        {
            throwed = false;
            transform.Translate(Vector3.zero);
            other.gameObject.GetComponent<MagicianBehaviour>().hit = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Destroy(itSelf);
        }
    }
}
