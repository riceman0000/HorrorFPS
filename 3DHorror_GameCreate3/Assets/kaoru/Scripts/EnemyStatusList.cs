using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    [CreateAssetMenu(menuName = "ParameterTables/Enemy",fileName ="EnemyParameter")]

    public class EnemyStatusList : ScriptableObject
    {
        public List<EnemyStatus> EnemyStatusL = new List<EnemyStatus>();
        
    }
    [System.Serializable]
    public class EnemyStatus
    {
        public EnemyTags.Tags Name;
        public int HP = 10, Attack = 5, Speed = 5;
    }

}