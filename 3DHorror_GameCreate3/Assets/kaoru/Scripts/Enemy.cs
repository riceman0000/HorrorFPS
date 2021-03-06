﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.AI;

namespace VRShooting
{
    public class Enemy : MonoBehaviour
    {
        SpawnManagerScript _SpawnManagerScript;
        Transform target;
        PlayerHP playerHP;
        Animator anim;
        EnemyStatus Zombie1;
        NavMeshAgent navMeshAgent;
        Subject<int> attackSubject = new Subject<int>();

        [SerializeField]
        float speed = 0;
        [SerializeField]
        float distValue = 1.7f;

        Vector3 vec = new Vector3(0, -0.7f, 0);
        int currentHP;
        bool isSafe = false;

        public bool PlayerIsSafe
        {
            get => isSafe;
            set => isSafe = value;
        }

        void Start()
        {
            _SpawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
            var playerObj = GameObject.Find("Player");
            target = playerObj.transform;
            playerHP = playerObj.GetComponent<PlayerHP>();
            anim = this.GetComponent<Animator>();
            EnemyStatusList esl = EnemyStatasManagement.Entity;
            Zombie1 = esl.EnemyStatusL[(int)EnemyTags.Tags.Enemy_Zombie1];
            currentHP = Zombie1.HP;
            navMeshAgent = GetComponent<NavMeshAgent>();
            attackSubject.ThrottleFirst(TimeSpan.FromSeconds(0.5f)).Subscribe((c) =>
            {
                playerHP.PlayerHPAdjustment(Zombie1.Attack);
            });
        }

        void Update()
        {
            AttackMotion();
        }
        void AttackMotion()
        {
            var distance = Vector3.Distance(transform.position, target.position);
            if (!PlayerIsSafe)//PlayerがSafeHouseに入っていたらNavMesh探索も攻撃も通さない
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                if (distance < distValue)
                {
                    anim.SetBool("isAttack", true);
                    attackSubject.OnNext(2);
                }
                else
                {
                    anim.SetBool("isAttack", false);
                    navMeshAgent.SetDestination(target.position);
                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + vec) - transform.position), 0.3f);
                    navMeshAgent.speed = speed;

                    //マップのレベルデザイン後にナビメッシュで動かす
                }
            }
            else //PlayerがSafeHouseにいないのでNavMeshをOffにする
            {
                GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
        public void HitDamage(int damage)
        {
            currentHP += damage;
            if (currentHP <= 0)
            {
                _SpawnManagerScript.GetComponent<SpawnManagerScript>().OnEnemyDeath();
                Destroy(this.gameObject);
                Debug.Log("Destroy" + this.gameObject.name);
            }
        }
    }
}