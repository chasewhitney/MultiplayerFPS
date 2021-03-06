﻿using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    private PlayerWeapon currentWeapon;

    private WeaponManager weaponManager;
    

    

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        

        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        } else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            } else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
            
        }
        
    }

    [Client]
    void Shoot()
    {

        Debug.Log("Shoot!");

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command] // Makes Unity understand that this was on the server
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);

        
    }
}
