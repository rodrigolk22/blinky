using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBehaviour : MonoBehaviour {

    public bool facingRight = true;
    public bool jump = false;
    public float moveForce = 700f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    private Transform groundCheck;
    private bool grounded = false;
    private Animator anim;
    public float moviment = 1;
    public bool hit = false;
    public bool died = false;
    public GameObject itSelf;
    public AudioSource audioSource;
    public AudioClip audioDie;
    public GameObject gameManager;
    public Transform collided;
    private int chanceToTurn;

    // Inicialiação das variáveis
    void Awake()
    {
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
        if (!facingRight)
        {
            moveForce *= -1;
        }
    }

    void Update () {
        // Checa posição no solo
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (died == false)
        {
            // Quando uma caixa arremesada encontra um inimigo (hit), o inimigo morre
            if (hit == true)
            {
                audioSource.PlayOneShot(audioDie);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                anim.SetTrigger("died");;
                gameManager.GetComponent<GameManager>().reduceEnemies();
                died = true;
                hit = false;
                StartCoroutine(erase());
            }
            else
            {
                // Ajuste de animação de pulo
                if (grounded)
                {
                    anim.ResetTrigger("jump_magi");
                }
            }
        }     
    }
    private void FixedUpdate()
    {
        // Se o nimigo não estiver morto, realiza ajustes de velocidade e orientação
        if(died == false)
        {
            anim.SetFloat("speed", Mathf.Abs(moviment));
            if (moviment * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * moviment * moveForce);
            }
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            if (moviment > 0 && !facingRight)
            {
                Flip();
            }
            else if (moviment < 0 && facingRight)
            {
                Flip();
            }
        }
    }
    // Colisões
    void OnTriggerEnter2D(Collider2D other)
    {
        if (died == false)
        {
            // Qunado colide com uma parede ele muda de direção
            if (other.gameObject.CompareTag("Wall"))
            {
                if (moviment == 1)
                {
                    moviment = -1;
                }
                else
                {
                    moviment = 1;
                }
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            // Quando colide com um coelho, faz o coelho para de seguir o herói
            if (other.gameObject.CompareTag("Rabbit"))
            {
                other.GetComponent<rabbitBehavior>().stopFollow();
                /* Evita ficar preso */
                StartCoroutine(checkStuck());
            }
            // Quando colide com o jogador, mata o jogador
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerControl>().died = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            // Quando colide com uma caixa que não foi arremessada tenta pular por cima
            if (other.gameObject.CompareTag("Box"))
            {
                if (other.gameObject.GetComponent<box>().grabbed == false && other.GetComponent<box>().throwed == false)
                {
                    /* Evita ficar preso */
                    StartCoroutine(checkStuck());
                } 
            }
        }
    }
    // Ajuste de orientação de imagem
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    /* De tempos em tempos checa se o inimigo ficou "preso" no mapa e 
     * realiza um pulo ou inverte a direção do movimento */
    IEnumerator checkStuck()
    {
        collided = transform;
        yield return new WaitForSeconds(2);
        if (collided = transform)
        {
            chanceToTurn = Random.Range(0, 2);
       
            if (chanceToTurn == 0)
            {
                anim.SetTrigger("jump_magi");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
            }
            else
            {
                Flip();
            }
            StartCoroutine(checkStuck());
        }
    }
    // Função que destrói o inimigo
    IEnumerator erase()
    {
        yield return new WaitForSeconds(3);
        Destroy(itSelf);
    }
}
