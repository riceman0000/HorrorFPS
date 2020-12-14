using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class SafeHouseFloor : MonoBehaviour
    {
        [SerializeField] Enemy enemyScript;
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                enemyScript.IsSafe = false;
            }
        }
        private void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                enemyScript.IsSafe = true;
            }
        }
    }
}
