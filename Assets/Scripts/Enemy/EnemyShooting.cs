using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{

    [SerializeField] private Transform Gun;
    [Header("Pistol Settings")]
    public bool pistol;
    public int pistolAmmo = 12;
    public float pistolFireRate;
    private float pistolFireRateTime;

    [Header("SMG Settings")]
    public bool SMG;
    public int smgAmmo = 30;
    public bool isReloading;
    public float smgFireRate;
    private float smgFireRateTime;

    [SerializeField] private float aggroRadius;
    
    public float bulletSpeed;

    public Transform shootPoint;

    private float ReadyToShoot;


    public GameObject player;

    private bool isAggro = false;

    private void Start()
    {
        player = GlobalReferences.Instance.GetPlayerObject();
        pistolFireRateTime = 1 / pistolFireRate;
        smgFireRateTime = 1 / smgFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAggro) //Aggro's and starts shooting if the player is within aggro distance
        {
            AimAtPlayer();

            if (!isReloading && (pistolAmmo == 0 || smgAmmo == 0)) //If enemy runs out of ammo, start reloading coroutine
            {
                StartCoroutine(Reload());
            }
            
            if (!isReloading) //Only tries to shoot if enemy is not mid-reload
            {
                if (pistol && pistolAmmo > 0) //If pistol ammo is above 0, allow the enemy to shoot at pistol fire rate
                {
                    if (Time.time > ReadyToShoot)
                    {
                        ReadyToShoot = Time.time + pistolFireRateTime;
                        Shoot();
                    }
                }
                else if (SMG && smgAmmo > 0) //If smg ammo is above 0, allow the enemy to shoot at smg fire rate
                {
                    if (Time.time > ReadyToShoot)
                    {
                        ReadyToShoot = Time.time + smgFireRateTime;
                        Shoot();
                    }
                }
            }
        }
    }

    private void AimAtPlayer()
    {
        Vector2 aimDirection = player.transform.position - Gun.position;
        Gun.right = aimDirection;
    }
    void Shoot()
    {
        if (pistol) //if pistol is equipped, consume pistol ammo
        {
            pistolAmmo--;
        }
        else if (SMG) //if smg is equipped, consume smg ammo
        {
            smgAmmo--;
        }

        Bullet BulletIns = BulletPool.Instance.GetBullet();
        BulletIns.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
        BulletIns.rb.AddForce(BulletIns.transform.right * bulletSpeed);
    }

    IEnumerator Reload() //Reloads based on what weapon is equipped
    {
        if (pistol)
        {
            isReloading = true;
            yield return new WaitForSeconds(1.5f);
            pistolAmmo = 12;
            isReloading = false;
        }
        else if (SMG)
        {
            isReloading = true;
            yield return new WaitForSeconds(2.5f);
            smgAmmo = 30;
            isReloading = false;
        }
    }

    public void SetAggro(bool value)
    {
        isAggro = value;
    }

    public float GetAggroRadius()
    {
        return aggroRadius;
    }
}
