using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRShooting
{
    public class StartButton : MonoBehaviour
    {
        public void ButtonClick()
        {
            SceneManaged.Instance.Test(SceneManaged.SceneNameTags.FPSplayscene.ToString());
        }
    }
}