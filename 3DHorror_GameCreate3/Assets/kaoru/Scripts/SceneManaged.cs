using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaged : MonoBehaviour
{
    private static SceneManaged instance;

    public static SceneManaged Instance
    {
        get
        {
            if (null == instance)
            {
                instance = (SceneManaged)FindObjectOfType(typeof(SceneManaged));
                if (null == instance)
                {
                    Debug.Log(" SceneManaged Instance Error : Not Create Insetance");
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SceneManaged");
        if (1 < obj.Length)
        {
            // 既に存在しているなら削除
            Destroy(gameObject);
        }
        else
        {
            // シーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// Scene移行を管理する唯一のメソッド。
    /// ScneNameTagsからscene名を簡単に取得できる。
    /// </summary>
    /// <param name="sceneTag">移行したいシーンの名前</param>
    public void SceneLoad(int sceneTag)
    {

        SceneManager.LoadScene(sceneTag);
    }
    /// <summary>
    /// シーンの名前を取得しやすく管理するenum
    /// </summary>
    public enum SceneNameTags
    {
        FPSplayscene = 0,
        MainMenu = 1,
        GameOver = 2,
        GameClear = 3
    }
}
