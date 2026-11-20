using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float minTime = 40f;
    [SerializeField] float maxTime = 50f;
    float timeStated;
    GameObject player;
    float spawnDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //timeStated = Random.Range(minTime, maxTime);
        timeStated = Random.Range(minTime, maxTime);
    }
    float time=0f;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= timeStated)
        {

            time = 0f;
            timeStated = Random.Range(minTime, maxTime);
            StartCoroutine(SpawnEnemyCoroutine());

        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        Debug.Log("Generando enemigo...");
        GameObject enemieSelected = enemies[Random.Range(0, enemies.Length)];
        BasicEnemyMovement enemyBEM = enemieSelected.GetComponent<BasicEnemyMovement>();
        Vector3 saveSpawnPosition = player.transform.position + (Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward * (enemyBEM.getMaxDistance() * 1 / 2)) + new Vector3(0, 2, 0);
        NavMeshHit hit;
        while (!NavMesh.SamplePosition(saveSpawnPosition, out hit, 10f, NavMesh.AllAreas))
        {
            Debug.Log(saveSpawnPosition);
            yield return null;
            saveSpawnPosition = player.transform.position + (Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.forward * (enemyBEM.getMaxDistance() * 1 / 2)) + new Vector3(0, 2, 0);
        }
        GameObject enemy = Instantiate(enemieSelected, hit.position, Quaternion.identity);
        enemy.GetComponent<NavMeshAgent>().Warp(hit.position);
        
    }
}
