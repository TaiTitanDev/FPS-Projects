using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100.0f;

    public void TakeDame(float amount)
    {
        health -= amount;
        if(health<=0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ENEMY die");
        Destroy(gameObject);
    }
}