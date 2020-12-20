using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

namespace VRShooting
{
    public class SpawnManagerScript : MonoBehaviour
    {
        public const string PATH = "StageFazeLists";
        private static StageFazeList _stageEntity;
        [SerializeField] GameObject spawnPoints;
        int childCount = 0;
        List<Transform> spawnPosTrans = new List<Transform>();
        public int enemyDeathCount = 0;
        public int nowFazeIndex = 0;
        [SerializeField] SafeHouseFloor _SafeHouseFloorScript;
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
        public void OnEnemyDeath()
        {
            enemyDeathCount++;
            Debug.Log(enemyDeathCount + "/" +
                StageEntity.Stages[nowFazeIndex].Enemys.Count + "体の敵を倒した");

            if (StageEntity.Stages[nowFazeIndex].Enemys.Count == enemyDeathCount)
            {
                enemyDeathCount = 0;
                Observable.Timer(TimeSpan.FromSeconds(3.0f)).Subscribe(_ =>
                {

                    if (nowFazeIndex + 1 == StageEntity.Stages.Count)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        SceneManaged.Instance.SceneLoad((int)SceneManaged.SceneNameTags.GameClear);
                    }
                    else
                    {
                        nowFazeIndex++;
                        SpawnEnemyOpportunity();
                    }

                    

                    Debug.Log("StartNextFaze!!!");

                }).AddTo(this);
            }

        }
        private void Start()
        {
            SetSpawnPositions();
            SpawnEnemyOpportunity();
        }
        private void SetSpawnPositions()
        {
            GameObject[] childSpawnPointsObj = GetComponentsInChildren<Transform>().
                   Select(t => t.gameObject).ToArray();
            foreach (var item in childSpawnPointsObj)
            {
                spawnPosTrans.Add(item.transform);
            }
            spawnPosTrans.RemoveRange(0, 2);
            childCount = spawnPoints.transform.childCount;
            Debug.Log(childCount);

        }
        /// <summary>
        /// Enemyをスポーンさせる機会を得たときに呼び出す。
        /// FazeNumberを基にSpawnPointにランダムに指定されたEnemyのinstanceを生成。
        /// FazeNumは1から正の整数(SpawnEnemyOpportunity(int fazeNum)で-1してelementを調整)
        /// *****spawnpointの数は暗黙的に五個置かれてるけど、StageFazeLists.Stages[].Enemysの
        ///       エレメント数が5を超えるとバグる。今後要修正*****
        /// </summary>
        public void SpawnEnemyOpportunity()
        {
            var nowStageData = StageEntity.Stages[nowFazeIndex];
            Shuffle(spawnPosTrans);//スポーンポイントをフェーズごとにシャッフルする。
            int child = 0;
            List<Enemy> nowFazeEnemyScripts = new List<Enemy>();

            foreach (var item in nowStageData.Enemys)//シャッフルされたスポーンポイントに敵をスポーンさせる。
            {
                int enemyTagNumber = (int)item.Tags;
                var spawn = this.transform.GetChild(0).transform
                    .GetChild(child).gameObject.transform;
                var enemy = Instantiate(StageEntity.Prefabs[enemyTagNumber].EnemyPrefab,
                    spawn.position, spawn.rotation);

                nowFazeEnemyScripts.Add(enemy.GetComponent<Enemy>());
                //EnemyがInstantiateされたタイミングで生成されたObjectをListに保存。
                //このListをSafeHouseFloorで取得して各々処理(Size分ループ)
                child++;
            }
            _SafeHouseFloorScript.CatchEnemyData(nowFazeEnemyScripts);
        }

        /// <summary>
        /// List<>のIndexをシャッフルするメソッド
        /// </summary>
        /// <param name="list"> シャッフルされるList </param>
        public void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }
    }
}