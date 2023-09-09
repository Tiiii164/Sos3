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
    [SerializeField] private Transform bulletPf;
    [SerializeField] private Transform bulletSpawnPos;

    //Get the ThirdPersonController script
    private ThirdPersonController thirPersonController;
    //Get the starterAssestsInput script
    private StarterAssetsInputs starterAssestsInput;

    private bool isAim = false;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private void Awake()
    {
        starterAssestsInput = GetComponent<StarterAssetsInputs>();
        thirPersonController = GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
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
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && isAim)
        {
            isAim = false;
            aimVirtualCamera.gameObject.SetActive(false);
            thirPersonController.SetSensitivity(normalSensitivity);
            thirPersonController.SetRotateOnMove(true);
        }
        if (isAim)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        if(starterAssestsInput.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawnPos.position).normalized;
            Instantiate
                (bulletPf, bulletSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
    }
}
