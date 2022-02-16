using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    private float xPos;
    private float zPos;

    private string enemyOwnorID;
    public string EnemyOwnorID { get => enemyOwnorID; set => enemyOwnorID = value; }

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float timeSpawn;

    private Coroutine coroutineSwpanEnemy = null;

    private void Start()
    {
        if(coroutineSwpanEnemy == null)
            coroutineSwpanEnemy = StartCoroutine(SpawnEnemy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameContans.ENEMY_TAG_NAME)
        {
            if (coroutineSwpanEnemy != null)
                StopCoroutine(coroutineSwpanEnemy);

            coroutineSwpanEnemy = StartCoroutine(SpawnEnemy());

            Enemy enemyOwnor = other.GetComponent<Enemy>();

            if(enemyOwnor != null && string.Equals(enemyOwnor.OwnorID, enemyOwnorID))
            {
                xPos = Random.Range(-20.0f, 20.0f);
                zPos = Random.Range(-20.0f, 20.0f);

                this.transform.position = new Vector3(xPos, 1.5f, zPos);
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(timeSpawn);
        var clone = Instantiate(enemyPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject, 1.0f);
    }
}
