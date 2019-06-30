using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public bool facingRight = true;
    public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    private float moviment;
    public Transform groundCheck;
    public bool grounded = false;
    public bool died = false;
    private Animator anim;
    private Transform pivot;
    private Vector3 playerPos;
    public GameObject player;
    public float offsetX = 0.3f;
    public float offsetY = 0.6f;
    public int rabbitCounter;
    public GameObject grabbedBox;
    public GameObject[] rabbits;
    private int rabbitsSize;
    public GameObject mainScripts;
    public GameObject exit;
    public AudioSource audioSource;
    public AudioClip audioJump;
    public AudioClip audioRabbit;
    public AudioClip audioDie;
    public AudioClip audioCrash;
    private Rigidbody2D rigidbd2D;
    // Inicializa as variáveis
    void Awake()
    {
        pivot = transform.Find("pivot");
        anim = GetComponent<Animator>();
        rigidbd2D = GetComponent<Rigidbody2D>();
        mainScripts = GameObject.Find("MainScripts");
    }

    void Update () {
        /*  Dispara um vetor apontando para baixo, caso a distância esteja curta,
         então o herói deve para de cair pos encontrou um chão ou a caixa */
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))
         || Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Box")))
        {
            grounded = true;
        }
        // Ajusta animação de movimento
        if (grounded)
        {
            anim.ResetTrigger("jump");
        }
        // Permite pular se o herói estiver asentado
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
            grounded = false;
            audioSource.PlayOneShot(audioJump);
        }
        /* Quando o herói morre o jogo libera os coelhos de segui-lo
        reseta os inimigos e renasce o herói na sáida */
        if (died)
        {
            died = false;
            anim.SetTrigger("die");
            audioSource.PlayOneShot(audioDie);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            moviment = 0;
            rabbitCounter = 0;
            rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
            foreach (GameObject rabbit in rabbits)
            {
                rabbit.GetComponent<rabbitBehavior>().following = false;
                rabbit.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
            StartCoroutine (respawnHeroPosition());
        }
    }

    void FixedUpdate()
    {
        // Caso o herói não esteja morto, procede as ações de movimento
        if (died == false) {
            moviment = Input.GetAxis("Horizontal");
            anim.SetFloat("speed", Mathf.Abs(moviment));
            // Ajuste do sinal da velocidade de acordo com a direção
            if (moviment * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * moviment * moveForce);
            }
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            // Ajuste da orientação da imagem
            if (moviment > 0 && !facingRight)
            {
                Flip();
            }
            else if (moviment < 0 && facingRight)
            {
                Flip();
            }
            // Ajuste da animação para o pulo
            if (jump)
            {
                anim.SetTrigger("jump");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
            // Atira uma caixa caso o herói esteja segurando ela
            if (Input.GetButtonDown("Fire1"))
            {
                if (anim.GetBool("grab") == true)
                {
                    audioSource.PlayOneShot(audioCrash);
                    anim.SetBool("grab", false);
                    if (grabbedBox)
                    {
                        grabbedBox.GetComponent<box>().grabbed = false;
                        grabbedBox.GetComponent<box>().throwed = true;
                        if (facingRight)
                        {
                            grabbedBox.GetComponent<box>().right = true;
                        }
                        else
                        {
                            grabbedBox.GetComponent<box>().right = false;
                        }
                    }
                    
                }
            }
        }
    }
    // Ajuste da orientação da imagem do herói
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    // Colisões
    void OnTriggerEnter2D(Collider2D other)
    {
        // Colidir com a caixa faz o herói pegá-la de ele ainda não está com uma
        if (other.gameObject.CompareTag("Box"))
        {
            if (anim.GetBool("grab") == false)
            {
                grabbedBox = other.gameObject;
                other.GetComponent<box>().grabbed = true;
                anim.SetBool("grab", true);
                other.GetComponent<box>().pivot = pivot;
            } 
        }
        // Quando o herói colide com o coelho, o coelho começa seguí-lo
        if (other.gameObject.CompareTag("Rabbit") && other.gameObject.GetComponent<rabbitBehavior>().following == false)
        {
            audioSource.PlayOneShot(audioRabbit);
            rabbitCounter += 1;
            rabbitBehavior rabbit = other.gameObject.GetComponent<rabbitBehavior>();
            rabbit.following = true;
            rabbit.GetComponent<rabbitBehavior>().posHero = new Vector3(player.transform.position.x, player.transform.localPosition.y, transform.localPosition.z);
            // Ajuste na animação
            other.gameObject.GetComponent<Animator>().SetTrigger("released");
            // Ajuste no posicionamenot e direção do coelho
            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                if (facingRight)
                {
                    rabbit.offset = new Vector3(-offsetX * (rabbitCounter * 0.1f), 0, 0);
                }
                else
                {
                    rabbit.offset = new Vector3(offsetX * (rabbitCounter * 0.1f), 0, 0);

                }
            }
            else
            {
                if (facingRight)
                {
                    rabbit.offset = new Vector3(-offsetX * (rabbitCounter * 0.1f), 0, 0);
                }
                else
                {
                    rabbit.offset = new Vector3(offsetX * (rabbitCounter * 0.1f), 0, 0);

                }
            }
             
        }
        // Quando o herói colide com a saída, se tiver coelhos, ele os entrega
        if (other.gameObject.CompareTag("Exit"))
        {
            rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
            int multiply = 1;
            foreach (GameObject rabbit in rabbits)
            {
                if (rabbit.GetComponent<rabbitBehavior>().following == true)
                {
                    mainScripts.GetComponent<GameManager>().increasePoints(100 * multiply);
                    rabbit.GetComponent<rabbitBehavior>().leave();
                    multiply++;
                    rabbitCounter--;
                    mainScripts.GetComponent<GameManager>().reduceRabbits();
                }
            }
        }
    }
    // Recria o herói na posição doa saída
    IEnumerator respawnHeroPosition()
    {
        rigidbd2D.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2);
        mainScripts.GetComponent<GameManager>().heroDied();
        transform.position = new Vector3(exit.transform.position.x, exit.transform.position.y, 0);
        anim.ResetTrigger("die");
        rigidbd2D.constraints = RigidbodyConstraints2D.None;
        rigidbd2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        mainScripts.GetComponent<GameManager>().resetEnemies();
    }
}
