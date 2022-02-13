using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float dame = 10.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float fireRate = 15.0f;

    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject fxBloodSplatter;

    private float nextTimeToFire = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1 / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDame(dame);
                Instantiate(fxBloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
