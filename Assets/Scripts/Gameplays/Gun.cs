using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GunType
{
    None = 0,
    Rifle = 1,
    Revolver = 2,
}

public class Gun : MonoBehaviour
{
    [SerializeField] private string ANIM_IDLE;
    [SerializeField] private string ANIM_SHOOT;
    [SerializeField] private string ANIM_RELOAD;

    [SerializeField] private int dame;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem fxMuzzelFlash;
    [SerializeField] private GameObject fxBloodSplatter;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int numberOfAmmo;
    [SerializeField] private float reloadTime;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textAmmo;
    [SerializeField] private GunType gunType;

    private string curentState;
    private int curentAmmo = 0;
    private bool isReloading = false;
    private float nextTimeToFire = 0.25f;

    public GunType GunType { get => gunType; private set => gunType = value; }

    private void OnEnable()
    {
        OnLoadData();
        OnCheckReload();
    }

    private void OnLoadData()
    {
        switch (gunType)
        {
            case GunType.None:
                break;
            case GunType.Rifle:
                numberOfAmmo = PlayerDataService.Instance.GetPlayerModel().NumberAmmoRifle;
                UpdateTextAmmo();
                break;
            case GunType.Revolver:
                numberOfAmmo = PlayerDataService.Instance.GetPlayerModel().NumberAmmoRevolver;
                UpdateTextAmmo();
                break;
        }
    }
    private void OnUpData()
    {
        switch (gunType)
        {
            case GunType.None:
                break;
            case GunType.Rifle:
                PlayerDataService.Instance.GetPlayerModel().NumberAmmoRifle = numberOfAmmo;
                break;
            case GunType.Revolver:
                PlayerDataService.Instance.GetPlayerModel().NumberAmmoRevolver = numberOfAmmo;
                break;
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading...");

        isReloading = true;
        AudioManager.Instance.OnReloadAudioSource();

        yield return new WaitForSeconds(reloadTime);

        if (numberOfAmmo > maxAmmo)
        {
            curentAmmo = maxAmmo;
            numberOfAmmo -= maxAmmo;
        }
        else
        {
            curentAmmo = numberOfAmmo;
            numberOfAmmo -= numberOfAmmo;
        }

        OnUpData();
        UpdateTextAmmo();
        ChangeAnimationState(newState: ANIM_IDLE);
        isReloading = false;
    }

    [System.Obsolete]
    public void Shoot()
    {
        if (isReloading) return;

        OnCheckReload();

        if (Time.time < nextTimeToFire) return;
        nextTimeToFire = Time.time + 1 / fireRate;

        // handler shoot actions.
        if (curentAmmo <= 0)
            return;

        AudioManager.Instance.OnShootAudioSource();
        ChangeAnimationState(newState: ANIM_SHOOT);

        fxMuzzelFlash.Play();
        curentAmmo--;
        UpdateTextAmmo();
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDame(dame);
                GameObject fxBloodSplatterGo = Instantiate(fxBloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                DestroyObject(fxBloodSplatterGo, 1.5f);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal);
            }
        }
    }

    public void OnCheckReload()
    {
        if (curentAmmo <= 0 && numberOfAmmo > 0)
        {
            ChangeAnimationState(newState: ANIM_RELOAD);
            StartCoroutine(Reload());
            return;
        }
    }

    public void SetAmmo(int numberAmmo)
    {
        numberOfAmmo = numberAmmo;
        UpdateTextAmmo();
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

    public void UpdateTextAmmo()
    {
        switch (gunType)
        {
            case GunType.None:
                break;
            case GunType.Rifle:
                numberOfAmmo = PlayerDataService.Instance.GetPlayerModel().NumberAmmoRifle;
                textAmmo.text = string.Format("{0}/{1}", curentAmmo, numberOfAmmo);
                break;
            case GunType.Revolver:
                numberOfAmmo = PlayerDataService.Instance.GetPlayerModel().NumberAmmoRevolver;
                textAmmo.text = string.Format("{0}/{1}", curentAmmo, numberOfAmmo);
                break;
        }
    }
}
