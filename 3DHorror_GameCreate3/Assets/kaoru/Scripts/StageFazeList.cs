using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRShooting
{
    [CreateAssetMenu(menuName ="StageList/Faze")]
    public class StageFazeList : ScriptableObject
    {
        public List<StageFazeElement> Stages;
    }

    [System.Serializable]
    public class StageFazeElement
    {
        
        //public int FazeNumber = 0;//Fazeの番号
        public List<EnemyEntity> Enemys;
        //Enemyの数(Enemys.size)と、Enemyの種類(Enemytags.Tags)を設定
    }

    [System.Serializable]
    public class EnemyEntity {
        public EnemyTags.Tags Tags;
    }




}