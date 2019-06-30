using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gerencia as ações do coelho "rabbitBehavior"
public class rabbitBehavior : MonoBehaviour {

    public bool following;
    public GameObject player;
    public Vector3 offset;
    public bool facingRight = true;
    private Animator anim;
    public Transform exit;
    public bool leaving = false;
    public int MoveSpeed = 4;
    public GameObject itSelf;
    public Vector3 posHero;
    public float offsetY;
    public BoxCollider2D gravityManager;
    public float speed = 2.0F;
    private float startTime;
    private float journeyLength;

    // Inicializa as variáveis
    void Start () {
        following = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, posHero);
    }

    private void FixedUpdate()
    {
        // Se o coelho não está partindo realiza demais ações, caso contrário ele se desloca até a saída
        if(leaving == false)
        {
            /* Caso o coelho esteja seguindo o herói, ele se desloca se o herói estiver
             * longe à uma distância determinada */
            if (following == true)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                gravityManager.enabled = false;
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;
                if (facingRight)
                {
                    posHero = new Vector3(player.transform.position.x + offset.x, player.transform.localPosition.y - offsetY, transform.localPosition.z);
                }
                else
                {
                    posHero = new Vector3(player.transform.position.x - offset.x, player.transform.localPosition.y - offsetY , transform.localPosition.z);
                }
                transform.position = Vector3.Lerp(transform.position, posHero, fracJourney);
            }
            else
            {
                GetComponent<Rigidbody2D>().gravityScale = 1;
                gravityManager.enabled = true;
            }
            // Realiza o ajuste na orientação do coelho
            float h = Input.GetAxis("Horizontal");
            if (following == true && h > 0 && !facingRight)
            {
                Flip();
            }
            else if (following == true && h < 0 && facingRight)
            {
                Flip();
            }
            // Ativa a animação caso o coelho esteja seguindo
            if (following == true)
            {
                if (player.GetComponent<Rigidbody2D>().velocity.x != 0)
                {
                    anim.SetTrigger("follow");
                }
                else
                {
                    anim.ResetTrigger("follow");
                }
            }
            else
            {
                anim.ResetTrigger("follow");
            }
        }
        else
        {
            transform.LookAt(exit);
            if (Vector3.Distance(transform.position, exit.position) > 0)
            {
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            }
        } 
    }
    // Função que inverte a orientação do coelho
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = new Vector3(theScale.x, transform.localScale.y, transform.localScale.z);
    }
    // Quando o coelho chega na saída ele deixa o jogo
    public void leave()
    {
        following = false;
        leaving = true;
        anim.ResetTrigger("follow");
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Destroy(itSelf);
    }
    /* Função faz com que o coelho para de seguir o herói porque o herói morreu
    ou porque o inimigo colidiu com o coelho */
    public void stopFollow()
    {
        if(following == true)
        {
            following = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.GetComponent<PlayerControl>().rabbitCounter--;
        }
    }
}
