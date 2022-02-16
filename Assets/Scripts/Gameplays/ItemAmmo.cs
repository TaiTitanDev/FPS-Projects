using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmmo : MonoBehaviour
{
    [SerializeField] private int numberOfAmmo;
    [SerializeField] private GunType gunType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameContans.PLAYER_TAG_NAME)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.GetAmmo(gunType, numberOfAmmo);
            Destroy(gameObject, 2.0f);
        }
    }
}
