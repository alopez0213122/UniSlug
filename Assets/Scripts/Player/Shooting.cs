using UnityEngine;
using System.Collections;
using TMPro;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform Gun;
    [Header("Pistol Settings")]
    public bool pistol = true;
    public int pistolAmmo = 12;
    public int pistolReserve = 36;
    public float pistolFireRate;

    [Header("SMG Settings")]
    public bool SMG = false;
    public int smgAmmo = 30;
    public int smgReserve = 30;
    public float smgFireRate;

    private bool isReloading = false;


    Vector2 mouseDirection;

    public GameObject Bullet;
    public float bulletSpeed;

    public Transform shootPoint;

    private float ReadyToShoot;


    public TextMeshProUGUI equippedWeapon;
    public TextMeshProUGUI AmmoText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gets Mouse and its direction
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = mousePos - (Vector2)Gun.position;
        FaceMouse();

        swapGuns();
        AmmoCount();

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (Input.GetMouseButton(0) && !isReloading) //Only allows player to shoot if not mid-reload
        {
            if (pistol && pistolAmmo > 0) //If pistol is equipped and ammo is not empty, shoot at the pistols fire rate
            {
                if (Time.time > ReadyToShoot)
                {
                    ReadyToShoot = Time.time + 1 / pistolFireRate;
                    Shoot();
                }
            }
            else if (SMG && smgAmmo > 0) //If smg is equipped and ammo is not empty, shoot at the smgs fire rate
            {
                if (Time.time > ReadyToShoot)
                {
                    ReadyToShoot = Time.time + 1 / smgFireRate;
                    Shoot();
                }
            }
            else
            {
                Debug.Log("No gun selected!");
            }
        }
    }

    void FaceMouse() //Faces Gun Towards mouse direction
    {
        Gun.transform.right = mouseDirection;
    }

    void swapGuns() //Swaps between Pistol and SMG with Num Keys
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equippedWeapon.text = "Pistol";
            pistol = true;
            SMG = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equippedWeapon.text = "SMG";
            SMG = true;
            pistol = false;
        }
    }

    void Shoot()
    {
        if (pistol)
        {
            pistolAmmo--;
        }
        else if (SMG)
        {
            smgAmmo--;
        }

        //Instantiate and shoot bullet
        GameObject BulletIns = Instantiate(Bullet, shootPoint.position, shootPoint.rotation);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * bulletSpeed);
    }

    private void AmmoCount() //Displays UI for Ammo
    {
        if (isReloading) 
        {
            AmmoText.text = "Reloading...";
        }
        else
        {
            if (pistol)
            {
                AmmoText.text = pistolAmmo + "/" + pistolReserve;
            }
            else if (SMG)
            {
                AmmoText.text = smgAmmo + "/" + smgReserve;
            }
        }

    }

    IEnumerator Reload()
    {
        if (pistol && pistolReserve >= 12) //If pistol is equipped and 12 reserve bullets are available, reload from reserve and discard 12 ammo
        {
            isReloading = true;
            yield return new WaitForSeconds(1.5f);
            pistolReserve -= 12;
            pistolAmmo = 12;
            isReloading = false;
        }
        else if (SMG && smgReserve >= 30) //If SMG is equipped and 30 reserve bullets are available, reload from reserve and discard 30 ammo
        {
            isReloading = true;
            yield return new WaitForSeconds(2.5f);
            smgReserve -= 30;
            smgAmmo = 30;
            isReloading = false;
        }
    }
}
