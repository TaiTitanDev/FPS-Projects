using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanteen : MonoBehaviour
{
    [SerializeField] private int bonushHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameContans.PLAYER_TAG_NAME)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.GetCanteen(bonushHealth);

            Destroy(gameObject, 2.0f);
        }
    }
}
