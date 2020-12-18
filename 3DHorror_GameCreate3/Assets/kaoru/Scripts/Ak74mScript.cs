using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

namespace VRShooting
{
    public class Ak74mScript : MonoBehaviour
    {
        public enum ShootMode { AUTO, SEMIAUTO }
        public bool shootEnabled = true;
        [SerializeField]
        ShootMode shootMode = ShootMode.AUTO;
        [SerializeField]
        int maxAmmo = 100;
        [SerializeField]
        int damage = 1;
        [SerializeField]
        float shootInterval = 0.15f;
        [SerializeField]
        float shootRange = 50;
        [SerializeField]
        Vector3 muzzleFlashScale;
        [SerializeField]
        GameObject muzzleFlashPrefab;
        [SerializeField]
        GameObject hitEffectPrefab;
        [SerializeField]
        Animator anim;
        bool shooting = false;
        int ammo;
        GameObject muzzleFlash;
        GameObject hitEffect;
        [SerializeField]
        Transform muzzlePosGameObj;
        [SerializeField]
        private int currentAmmo;
        bool canShoot = true;
        bool reLoadFlag = true;
        [SerializeField]
        Text ammoText;
        [SerializeField]
        Transform cameraPos;
        public int Ammo
        {
            set
            {
                ammo = Mathf.Clamp(value, 0, maxAmmo);
                currentAmmo = ammo;
                ammoText.text = currentAmmo.ToString() + " / " + maxAmmo;
            }
            get
            {
                return ammo;
            }
        }
        void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
            {
                if (!canShoot)
                {
                    canShoot = true;
                }
            });


            InitGun();
        }
        void Update()
        {
            if (shootEnabled & GetInput())
            {
                StartCoroutine(ShootTimer());
            }

        }
        void InitGun()
        {
            Ammo = maxAmmo;
        }
        bool GetInput()
        {
            switch (shootMode)
            {
                case ShootMode.AUTO:
                    return Input.GetMouseButton(0);
                case ShootMode.SEMIAUTO:
                    return Input.GetMouseButtonDown(0);
            }
            return false;
        }


        IEnumerator ShootTimer()
        {
            if (!shooting)
            {
                shooting = true;


                if (Ammo <= 0)//リロード
                {
                    ReLoad();
                }
                else
                {
                    //マズルフラッシュON
                    if (muzzleFlashPrefab != null)
                    {
                        if (muzzleFlash != null)
                        {
                            muzzleFlash.SetActive(true);
                        }
                        else
                        {
                            muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                            muzzleFlash.transform.SetParent(muzzlePosGameObj);
                            muzzleFlash.transform.localScale = muzzleFlashScale;
                            muzzleFlash.transform.localPosition = new Vector3(0f, 0f, 0f);
                        }
                    }
                    Shoot();
                }

                yield return new WaitForSeconds(shootInterval);
                //マズルフラッシュOFF
                if (muzzleFlash != null)
                {
                    muzzleFlash.SetActive(false);
                }
                //ヒットエフェクトOFF
                if (hitEffect != null)
                {
                    if (hitEffect.activeSelf)
                    {
                        hitEffect.SetActive(false);
                    }
                }
                shooting = false;
            }
            else
            {
                yield return null;
            }
        }
        void Shoot()
        {
            Debug.Log("Shoot()");
            anim.SetTrigger("Fire");
            Ray ray = new Ray(cameraPos.position, cameraPos.forward);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
            {
                //レイを飛ばして、ヒットしたオブジェクトの情報を得る
                if (Physics.Raycast(ray, out hit, shootRange))
                {
                    //ヒットエフェクトON
                    if (hitEffectPrefab != null)
                    {
                        //muzzleFlash.transform.position = muzzlePosGameObj.position;
                        if (hitEffect != null)
                        {
                            hitEffect.transform.position = hit.point;
                            hitEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                            hitEffect.SetActive(true);
                        }
                        else
                        {
                            hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
                        }
                    }
                    if (hit.collider.tag == EnemyTags.Tags.Enemy_Zombie1.ToString())
                    {
                        Debug.Log("Zombie1当たり");
                        var enemy = hit.collider.GetComponent<Enemy>();
                        if (enemy != null && canShoot)
                        {
                            enemy.HitDamage(-1);
                        }
                        else { Debug.LogError("Hitしたはずのオブジェクト情報が取れていないよ"); }

                    }
                    //敵へのダメージ処理記述
                }
                Ammo--;
            }
        }
        void ReLoad()
        {
            if (reLoadFlag)
            {
                Debug.Log("Re Load Now");
                Observable.Timer(TimeSpan.FromSeconds(2.0f)).Subscribe(_ =>
                {
                    Ammo = maxAmmo;
                    Debug.Log("ReLoadCompleted");
                    reLoadFlag = true;
                });
            }
            reLoadFlag = false;
        }
    }
}