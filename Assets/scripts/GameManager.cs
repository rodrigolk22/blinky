using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int totalEnemies;
    public static int remainingEnemies;
    public GameObject[] respawns;
    public int respawnTime;
    public int sizeOfRespawns;
    public GameObject[] rabbits;
    public static int remainingRabbits;
    private bool createEnemy;
    public GameObject exit;
    public GameObject hero;
    public GameObject totalPoints;
    public GameObject lifeCounter;
    public GameObject rabbitText;
    public GameObject levelText;

    void Start() {
        levelText = GameObject.Find("levelText");
        levelText.GetComponent<Text>().text = "level "+Data.actualLevel;
        totalPoints = GameObject.Find("totalPoints");
        lifeCounter = GameObject.Find("lifeCounter");
        lifeCounter.GetComponent<Text>().text = "x" + Data.lifeCounter;
        respawns = GameObject.FindGameObjectsWithTag("RespawnZone");
        sizeOfRespawns = respawns.Length;
        totalEnemies = sizeOfRespawns;
        remainingEnemies = 0;
        respawnTime = 4;
        rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        remainingRabbits = rabbits.Length;
        rabbitText = GameObject.Find("rabbitText");
        rabbitText.GetComponent<Text>().text = remainingRabbits + "/" + remainingRabbits;
        createEnemy = false;
        hero.transform.position = exit.transform.position;
    }

    // Popula os inimigos
    void FixedUpdate() {
        if (createEnemy == false && remainingEnemies < totalEnemies)
        {
            StartCoroutine("respawnOrder");
            createEnemy = true;
        }
    }
    // Cria uma inimigo numa zona de reaparecimento aleatória
    IEnumerator respawnOrder()
    {
        remainingEnemies++;
        yield return new WaitForSeconds(respawnTime);
        int randSpot = Random.Range(0, sizeOfRespawns);
        respawns[randSpot].GetComponent<respawnZone>().respawnEnemy();
        createEnemy = false;
    }
    public void reduceEnemies(){
        remainingEnemies--;
    }
    // Faz a contabilização dos coelhos e procede à próxima tela ou fim do jogo
    public void reduceRabbits()
    {
        remainingRabbits--;
        rabbitText.GetComponent<Text>().text = remainingRabbits + "/" + rabbits.Length;
        /* Condicao de vitoria */
        if (remainingRabbits <= 0)
        {
            /* Verifica final do jogo ou procede ao próximo level */
            if (Data.actualLevel + 3 == SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                Data.actualLevel++;
                SceneManager.LoadScene(Data.actualLevel);
            }

        }
    }
    // Remove todos os inimigos do cenário
    public void resetEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }   
        remainingEnemies = 0;
    }
    // Contabiliza pontos e atrualiza o marcador da tela
    public void increasePoints(int points)
    {
        Data.totalPoints += points;
        totalPoints.GetComponent<Text>().text = Data.totalPoints.ToString();
    }
    // Checa final do jogo por falta de vidas do jogador
    public void heroDied()
    {
        /* checa se o heroi morreu */
        if (Data.lifeCounter <= 0)
        {
            /* procede o fim do jogo */
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            /* Procede o reset do jogador */
            Data.lifeCounter--;
            lifeCounter.GetComponent<Text>().text = "x"+Data.lifeCounter;
        }

    }
}
