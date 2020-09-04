using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRShooting
{
    public class SpawnManagerScript : MonoBehaviour
    {
        public const string PATH = "StageFazeLists";
        private static StageFazeList _stageEntity;
        [SerializeField] GameObject Enemy_Zombie1;
        [SerializeField] GameObject Enemy_Zombie2;
        [SerializeField] GameObject spawnPoints;
        private List<GameObject> enemy_obj = new List<GameObject>();
        int childCount = 0;
        public static StageFazeList StageEntity
        {
            get
            {
                //初アクセス時にロード
                if (_stageEntity == null)
                {

                    _stageEntity = Resources.Load<StageFazeList>(PATH);


                    //ロード出来なかった場合はエラーログを表示
                    if (_stageEntity == null)
                    {
                        Debug.LogError(PATH + " not found");
                    }
                }

                return _stageEntity;
            }
        }
        private void Start()
        {
            enemy_obj.Add(Enemy_Zombie1);
            enemy_obj.Add(Enemy_Zombie2);

            childCount = spawnPoints.transform.childCount;
            SpawnEnemyOpportunity(1);
            Debug.Log(childCount);
            Debug.Log(enemy_obj[0]);
            Debug.Log(enemy_obj[1]);
        }
        //そのFazeのEnemys.sizega0になった時に、SpawnEnemyOpportunity()を呼び出すクラス。

        /// <summary>
        /// Enemyをスポーンさせる機会を得たときに呼び出す。
        /// FazeNumberを基にSpawnPointにランダムに指定されたEnemyのinstanceを生成。
        /// FazeNumは1から正の整数(SpawnEnemyOpportunity(int fazeNum)で-1してelementを調整)
        /// </summary>
        public void SpawnEnemyOpportunity(int fazeNum)
        {
            var a = StageEntity.Stages[fazeNum - 1];
            List<GameObject> g = Dio(a);
            foreach (var item in g)
            {
                Random.Range(0,childCount);
                Instantiate(item, transform.position, transform.rotation);
            }
            
            //transformはspawnpointをランダムに選択

        }
        private List<GameObject> Dio(StageFazeElement n)
        {
            List<GameObject> g = new List<GameObject>();
            for (int i = 0; i < n.Enemys.Count; i++)
            {
                for (int j = 0; j < enemy_obj.Count; j++)
                {
                    if (n.Enemys[i].Tags.ToString() == enemy_obj[j].name)
                    {
                        g.Add(enemy_obj[j]);
                    }
                }
            }
            return g;
        }
    }
}