﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace VRShooting
{
    public class Enemy : MonoBehaviour
    {
        SpawnManagerScript _SpawnManagerScript;
        Transform target;
        PlayerHP playerHP;
        Animator anim;
        EnemyStatus Zombie1;

        Subject<int> attackSubject = new Subject<int>();

        [SerializeField]
        float speed = 0;
        [SerializeField]
        float distValue = 1.7f;

        Vector3 vec = new Vector3(0, -0.7f, 0);
        int currentHP;

        void Start()
        {
            _SpawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
            var playerObj = GameObject.Find("Player");
            target = playerObj.transform;
            playerHP = playerObj.GetComponent<PlayerHP>();
            anim = this.GetComponent<Animator>();
            EnemyStatusList esl = ESManagement.Entity;
            Zombie1 = esl.EnemyStatusL[(int)EnemyTags.Tags.Enemy_Zombie1];
            currentHP = Zombie1.HP;

            attackSubject.ThrottleFirst(TimeSpan.FromSeconds(0.5f)).Subscribe((c) =>
            {
                playerHP.A(Zombie1.Attack);
            });
        }

        void Update()
        {
            AttackMotion();
        }
        void AttackMotion()
        {
            var distance = Vector3.Distance(transform.position, target.position);
            if (distance < distValue)
            {
                anim.SetBool("isAttack", true);
                attackSubject.OnNext(2);
            }
            else
            {
                anim.SetBool("isAttack", false);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + vec) - transform.position), 0.3f);
                transform.position += transform.forward * speed * 0.001f;
                //マップのレベルデザイン後にナビメッシュで動かす
            }
        }
        void HP(int hp)
        {
            currentHP += hp;
            if (currentHP <= 0)
            {
                _SpawnManagerScript.GetComponent<SpawnManagerScript>().EnemyDeathCount();
                Destroy(this.gameObject);
                Debug.Log("Destroy" + this.gameObject.name);
            }
        }
        public void HitDamage(int damage)
        {
            HP(damage);
        }
    }
}