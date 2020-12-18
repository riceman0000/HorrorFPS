using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneLoaderFromGameClear : MonoBehaviour
{
    public void SceneLoad_MainMenu()
    {
        SceneManaged.Instance.SceneLoad((int)SceneManaged.SceneNameTags.MainMenu);
    }

}
