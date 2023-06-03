using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float maxZ, minZ;
    public GameObject[] enemy;
    public int numberOfEnemies;
    public float spawnTime;

    private int currentEnemies;
    
    void Update() {
        {
            if(currentEnemies >= numberOfEnemies)
            {
                //procura enemigos no jogo
                int enemies = FindObjectsOfType<Enemy>().Length;
                if(enemies <= 0)
                {
                    //retorna movimento da camera
                    FindObjectOfType<CameraFollow>().maxXAndY.x = 200;
                    //desativa spawner
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void SpawnEnemy()
    {
        //sorteia em qual lado da tela enemigo spawna
        bool positionX = Random.Range(0,2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);
        if(positionX)
        {
            spawnPosition = new Vector3(transform.position.x + 10, 0, spawnPosition.z);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x - 10, 0, spawnPosition.z);
        }
        //spawna enemigo aleatorio do array no spawnPosition, sem configurar rotacao
        Instantiate(enemy[Random.Range(0,enemy.Length)], spawnPosition, Quaternion.identity);

        currentEnemies++;
        if(currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            //trava posicao da camera
            FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
            SpawnEnemy();
        }
    }
}
