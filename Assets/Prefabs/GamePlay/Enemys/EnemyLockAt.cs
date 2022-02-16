using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockAt : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameContans.PLAYER_TAG_NAME)
        {
            enemy.TargetGameObject = other.gameObject;
        }
    }
}
