using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class GameOverText : MonoBehaviour
    {
        bool isGetAnyKey = false;
        [SerializeField]
        GameObject AnyClickTextObj;
        public void AnimEvent()
        {
            isGetAnyKey = true;
            AnyClickTextObj.SetActive(true);
        }
        private void Update()
        {
            if (isGetAnyKey)
            {
                if (Input.anyKey)
                {
                    SceneManaged.Instance.SceneLoad((int)SceneManaged.SceneNameTags.MainMenu);
                }
            }

        }
    }
}
