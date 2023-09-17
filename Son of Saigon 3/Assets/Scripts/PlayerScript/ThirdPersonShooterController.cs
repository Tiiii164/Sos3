using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    // Get the VirtualCamera
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform rifeBulletPf;
    [SerializeField] private Transform pistolBulletPf;
    [SerializeField] private Transform rifeBulletSpawnPos;
    [SerializeField] private Transform pistolBulletSpawnPos;

    //Get the ThirdPersonController script
    private ThirdPersonController thirPersonController;
    //Get the starterAssestsInput script
    private StarterAssetsInputs starterAssestsInput;

    public bool isAim = false;
    private bool spawnBullet = true;
    public float lastPistolShotTime = 0f;
    public float lastRifeShotTime = 0f;
    public float delaySpawnRifeBullet = 0.05f;
    public float delaySpawnPistolBullet = 0.1f;
    private Animator animator;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private void Awake()
    {
        starterAssestsInput = GetComponent<StarterAssetsInputs>();
        thirPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Aimming();
        if(thirPersonController.hasRife)
        {
            CheckRifeFire();
        }
        if(thirPersonController.hasPistol)
        {
            CheckPistolFire();
        }  
    }

    public void Aimming()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isAim)
        {
            isAim = true;
            aimVirtualCamera.gameObject.SetActive(true);
            thirPersonController.SetSensitivity(aimSensitivity);
            thirPersonController.SetRotateOnMove(false);
            animator.SetBool("Aimming", true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isAim)
        {
            isAim = false;
            aimVirtualCamera.gameObject.SetActive(false);
            thirPersonController.SetSensitivity(normalSensitivity);
            thirPersonController.SetRotateOnMove(true);
            animator.SetBool("Aimming", false);
        }
        if (isAim)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
    }

    public void CheckRifeFire()
    {
        if (starterAssestsInput.shoot && spawnBullet && thirPersonController.hasRife)
        {
            float timeSinceLastShot = Time.time - lastRifeShotTime;
            if(timeSinceLastShot >= delaySpawnRifeBullet)
            {
                lastRifeShotTime = Time.time;
                RifeShooting();
                animator.SetBool("Shooting", true);
            }
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
    }

    public void CheckPistolFire()
    {
        if (starterAssestsInput.shoot && spawnBullet && thirPersonController.hasPistol)
        {
            float timeSinceLastShot = Time.time - lastPistolShotTime;
            if (timeSinceLastShot >= delaySpawnPistolBullet)
            {
                lastPistolShotTime = Time.time;
                PistolShooting();
            }
        }
    }

    public void RifeShooting()
    {
        if (spawnBullet)
        {
            Vector3 aimDir = (mouseWorldPosition - rifeBulletSpawnPos.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(aimDir, Vector3.up)
                * Quaternion.Euler(90f, 0, 0);
            Instantiate(rifeBulletPf, rifeBulletSpawnPos.position, rotation);
        }
    }

    public void PistolShooting()
    {
        if (spawnBullet)
        {
            Vector3 aimDir = (mouseWorldPosition - pistolBulletSpawnPos.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(aimDir, Vector3.up)
                * Quaternion.Euler(90f, 0, 0);
            Instantiate(pistolBulletPf, pistolBulletSpawnPos.position, rotation);
        }
    }
}
