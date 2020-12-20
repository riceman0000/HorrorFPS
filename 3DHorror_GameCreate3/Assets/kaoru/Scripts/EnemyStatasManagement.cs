using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class EnemyStatasManagement : MonoBehaviour
    {
        public const string PATH = "EnemyParameter";
        private static EnemyStatusList _entity;
        public static EnemyStatusList Entity
        {
            get
            {
                //初アクセス時にロード
                if (_entity == null)
                {
                    _entity = Resources.Load<EnemyStatusList>(PATH);
                    

                    //ロード出来なかった場合はエラーログを表示
                    if (_entity == null)
                    {
                        Debug.LogError(PATH + " not found");
                    }
                }

                return _entity;
            }
        }
    }
}