using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class SafeHouseFloor : MonoBehaviour
    {
        List<Enemy> enemyObjs;
        bool isSafeFrag = false;
        private void Start()
        {
            SendIsSafe();
        }
        public void CatchEnemyData(List<Enemy> enemies)
        {
            enemyObjs = enemies;
        }
        private void OnTriggerStay(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (!isSafeFrag)
                {
                    isSafeFrag = true;
                    SendIsSafe();
                }
            }
        }
        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (!isSafeFrag)
                {
                    isSafeFrag = true;
                    SendIsSafe();
                }
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                if (isSafeFrag)
                {
                    isSafeFrag = false;
                    SendIsSafe();
                }

            }
        }
        private void SendIsSafe()
        {
            foreach (var i in enemyObjs)
            {
                i.PlayerIsSafe = isSafeFrag;
            }
        }
    }
}
