using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] [Range(0.0f, 0.5f)] private float moveSmoothTime = 0.25f;
    [SerializeField] [Range(0.0f, 0.5f)] private float mouseSmoothTime = 0.03f;
    [SerializeField] private List<Gun> guns;

    private Gun curentGun;
    private int selectCurentGun = 0;

    public float jumpSpeed = 16.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private bool isLockCursor;

    private float cameraPitch = 100.0f;
    private CharacterController characterController;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private PlayerModel playerModel;
    private bool isDie;

    // Start is called before the first frame update
    private void Start()
    {
        playerModel = PlayerDataService.Instance.GetPlayerModel();
        characterController = GetComponent<CharacterController>();
        curentGun = guns[selectCurentGun];
        isDie = false;
        WeaponUpdate();
        if (isLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDie) return;
        WeaponSwitching();
        MouseLook();
        Movement();
        Shooting();
        Jump();
    }

    private void MouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void Movement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Shooting()
    {
        if (Input.GetButton("Fire1"))
        {
            curentGun.Shoot();
        }

        if(Input.GetButtonUp("Fire1"))
        {
            curentGun.ResetAnimation();
        }
    }

    private void WeaponSwitching()
    {
        int previousSelectGun = selectCurentGun;

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(selectCurentGun >= guns.Count - 1)
            {
                selectCurentGun = 0;
            }
            else
            {
                selectCurentGun++;
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(selectCurentGun <= 0)
            {
                selectCurentGun = guns.Count - 1;
            }
            else
            {
                selectCurentGun--;
            }
        }

        if(previousSelectGun != selectCurentGun)
        {
            curentGun = guns[selectCurentGun];
            WeaponUpdate();
        }
    }

    private void WeaponUpdate()
    {
        int i = 0;
        foreach (var gun in guns)
        {
            if(i == selectCurentGun)
                gun.gameObject.SetActive(true);
            else
                gun.gameObject.SetActive(false);
            i++;
        }
    }

    public void TakeDame(int amount)
    {
        SetHeath(-amount);
        AudioManager.Instance.OnTakeDameAudioSource();
        EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventUpdateHealth));
        if (playerModel.Health <= 0)
        {
            Die();
        }
    }    
    
    public void GetCanteen(int amount)
    {
        AudioManager.Instance.OnGetCanteenAudioSource();
        SetHeath(amount);
        EventSystem.Instance.Announce(new Message(SystemEventType.SystemEventUpdateHealth));
    }

    private void SetHeath(int amount)
    {
        playerModel.Health += amount;

        PlayerDataService.Instance.SetPlayerModel(playerModel);
    }

    public void GetAmmo(GunType gunType, int amount)
    {
        if(gunType == GunType.Rifle)
        {
            playerModel.NumberAmmoRifle += amount;
        }

        if(gunType == GunType.Revolver)
        {
            playerModel.NumberAmmoRevolver += amount;
        }

        AudioManager.Instance.OnGetAmmoAudioSource();
        PlayerDataService.Instance.SetPlayerModel(playerModel);
        curentGun.UpdateTextAmmo();
    }

    private void Die()
    {
        GamePlayController.Instance.EndGame();
        isDie = true;
    }
}
