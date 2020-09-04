using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace VRShooting
{
    public class SpawnManagerScript : MonoBehaviour
    {
        public const string PATH = "StageFazeLists";
        private static StageFazeList _stageEntity;
        [SerializeField] GameObject spawnPoints;
        private List<GameObject> enemy_obj = new List<GameObject>();
        int childCount = 0;
        List<Transform> spawnPosTrans = new List<Transform>();

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
            GameObject[] childSpawnPointsObj = GetComponentsInChildren<Transform>().
                Select(t => t.gameObject).ToArray();
            foreach (var item in childSpawnPointsObj)
            {
                spawnPosTrans.Add(item.transform);
            }
            spawnPosTrans.RemoveRange(0,2);
            childCount = spawnPoints.transform.childCount;
            SpawnEnemyOpportunity(1);//テスト用にfaze番号1を引数に入れている
            Debug.Log(childCount);
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
                Shuffle(spawnPosTrans);
            int g = 0;
            foreach (var item in a.Enemys)
            {
                
                int s = (int)item.Tags;
                var spawn = this.transform.GetChild(0).transform.GetChild(g).gameObject.transform;
                //Random.Range(0,childCount)の値を使ってSpawnpointを指定する。
                Instantiate(StageEntity.Prefabs[s].EnemyPrefab,
                    spawn.position, spawn.rotation);
                g++;
            }


            //transformはspawnpointをランダムに選択

        }
        public void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
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