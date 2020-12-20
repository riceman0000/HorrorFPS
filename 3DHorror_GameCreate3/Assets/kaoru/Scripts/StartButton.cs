using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class StartButton : MonoBehaviour
    {
        public void ButtonClick()
        {
            SceneManaged.Instance.SceneLoad((int)SceneManaged.SceneNameTags.FPSplayscene);
        }
    }
}