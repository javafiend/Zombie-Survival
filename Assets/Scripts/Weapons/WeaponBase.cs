using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FireMode
{
    SemiAuto,
    FullAuto
}

public class WeaponBase : MonoBehaviour {

    protected Animator animator;
    protected AudioSource audioSource;
    protected bool fireLock = false;
    protected bool canShoot = false;
    protected bool isReloading = false;

    [Header("Object Reference")]

    public ParticleSystem muzzleflash;
    public Transform shootPoint;

    [Header("UI References")]
    public Text weaponNameText;
    public Text ammoText;

    [Header("Sound References")]

    public AudioClip fireSound;
    public AudioClip dryFireSound;
    public AudioClip drawSound;
    public AudioClip magOutSound;
    public AudioClip magInSound;
    public AudioClip boltSound;
    [Header("Weapon Attributes")]

    public FireMode fireMode = FireMode.FullAuto;
    public float damage = 20f;
    public float fireRate = 0.25f;
    public int bulletsInClip = 12;
    public int bulletsLeft = 100;
    public int clipSize = 12;
    public int maxAmmo = 100;

    // Use this for initialization
    void Start()
    {
        Transform inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        weaponNameText = inGameUITransform.Find("WeaponNameText").GetComponent<Text>();
        ammoText = inGameUITransform.Find("AmmoText").GetComponent<Text>();
        
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        bulletsInClip = clipSize;
        bulletsLeft = maxAmmo;

        Invoke("EnableWeapon", 1f);
    }

    public void UpdateText()
    {
        weaponNameText.text = transform.name;
        ammoText.text = "Ammo: " + bulletsInClip + " / " + bulletsLeft;
    }

    string GetWeaponName()
    {
        string weaponName = "";
        if(this is Police9mm)
        {
            weaponName = "Police 9mm";
        }
        else if(this is PortableMagnum)
        {
            weaponName = "Portable Magnum";
        }
        else if(this is Compact9mm)
        {
            weaponName = "Compact 9mm";
        }
        else if (this is UMP45)
        {
            weaponName = "UMP45";
        }
        else if (this is StovRifle)
        {
            weaponName = "AK 47";
        }
        else
        {
            throw new System.Exception("Unknown Weapon");
        }

        return weaponName;
    }
    void EnableWeapon()
    {
        canShoot = true;
    }
    // Update is called once per frame
    void Update()
    { 
        if(fireMode == FireMode.FullAuto && Input.GetButton("Fire1"))
        {
            CheckFire();
        }
        else if(fireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            CheckFire();
        }
        if (Input.GetButtonDown("Reload"))
        {
            CheckReload();
        }
    }

    public void Select()
    {
        isReloading = false;
        Invoke("UpdateText", Time.deltaTime);
    }

    void CheckFire()
    {
        if (!canShoot)
            return;
        if (isReloading)
            return;
        if (fireLock)
            return;
        if(bulletsInClip > 0)
        {
            Fire();
        }
        else
        {
            DryFire();
        }
        
    }

    void Fire()
    {
       
        audioSource.PlayOneShot(fireSound);
        fireLock = true;

        DetectHit();

        muzzleflash.Stop();
        muzzleflash.Play();

        PlayFireAnimation();

        bulletsInClip--;
        UpdateText();

        StartCoroutine(CoResetFireLock());
    }

    void DetectHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            if(hit.transform.CompareTag("Enemy"))
            {
                Health health = hit.transform.GetComponent<Health>();
                if(health == null)
                {
                    throw new System.Exception("Cant find Health Component on Enemy");
                }
                else
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }

    public virtual void PlayFireAnimation()
    {
        animator.CrossFadeInFixedTime("Fire", 0.1f);
    }

    void DryFire()
    {
        audioSource.PlayOneShot(dryFireSound);
        fireLock = true;

        StartCoroutine(CoResetFireLock());
    }
    IEnumerator CoResetFireLock()
    {
        yield return new WaitForSeconds(fireRate);
        fireLock = false;
    }

    void CheckReload()
    {
        if(bulletsLeft > 0 && bulletsInClip < clipSize)
            Reload();
    }

    void Reload()
    {
        if (isReloading)
            return;
        isReloading = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void ReloadAmmo()
    {
        int bulletsToLoad = clipSize - bulletsInClip;
        int bulletsToSub = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToSub;
        bulletsInClip += bulletsToSub;

        UpdateText();
    }
    public void OnDraw()
    {
        audioSource.PlayOneShot(drawSound);
    }

    public void OnMagOut()
    {
        audioSource.PlayOneShot(magOutSound);
    }

    public void OnMagIn()
    {
        ReloadAmmo();
        audioSource.PlayOneShot(magInSound);
    }

    public void OnBoltForwarded()
    {
        audioSource.PlayOneShot(boltSound);
        Invoke("ResetIsReloading", 1f);
    }
    void ResetIsReloading()
    {
        isReloading = false;
    }
}
