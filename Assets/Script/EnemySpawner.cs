using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnRate;
    [SerializeField] float spawnRadius;
    [SerializeField] float minDistFromPlayer;

    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    IEnumerator spawnEnemies()
    {
        // ill do this later lol
        yield return new WaitForSeconds(spawnRate);
    }

    Vector2 findSpawnPos()
    {
        Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;
        float distFromPlayer = Vector2.Distance(player.transform.position, spawnPos);

        if (distFromPlayer < minDistFromPlayer)
        {
            return findSpawnPos();
        } else
        {
            return spawnPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minDistFromPlayer);
    }
}
