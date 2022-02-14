using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private string ANIM_IDLE;
    [SerializeField] private string ANIM_SHOOT;
    [SerializeField] private string ANIM_RELOAD;

    [SerializeField] private float dame = 25.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float fireRate = 10.0f;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem fxMuzzelFlash;
    [SerializeField] private GameObject fxBloodSplatter;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float reloadTime = 2.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textAmmo;

    private string curentState;
    private int curentAmmo = -1;
    private bool isReloading = false;
    private float nextTimeToFire = 0.25f;

    private void Start()
    {
        if(curentAmmo == -1)
        {
            curentAmmo = maxAmmo;
        }
        UpdateTextAmmo();

    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        curentAmmo = maxAmmo;
        UpdateTextAmmo();
        ChangeAnimationState(newState: ANIM_IDLE);
        isReloading = false;
    }

    public void Shoot()
    {
        if (isReloading) return;

        if (curentAmmo <= 0)
        {
            ChangeAnimationState(newState: ANIM_RELOAD) ;
            StartCoroutine(Reload());
            return;
        }

        if (Time.time < nextTimeToFire) return;
        nextTimeToFire = Time.time + 1 / fireRate;

        // handler shoot actions.
        ChangeAnimationState(newState: ANIM_SHOOT);

        fxMuzzelFlash.Play();
        curentAmmo--;
        UpdateTextAmmo();
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDame(dame);
                Instantiate(fxBloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal);
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (string.Equals(curentState, newState))
            return;

        animator.Play(newState);
        curentState = newState;
    }

    public void ResetAnimation()
    {
        if (string.Equals(curentState, ANIM_RELOAD))
            return;
        ChangeAnimationState(newState: ANIM_IDLE);
    }

    private void UpdateTextAmmo()
    {
        textAmmo.text = string.Format("{0}/{1}", curentAmmo, maxAmmo);
    }
}
