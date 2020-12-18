using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    //HP管理
    [SerializeField]int playerHp;
    [SerializeField] Slider slider;

    public void PlayerHPAdjustment(int Damage)
    {
        playerHp -= Damage;
        slider.value = playerHp;
        if (playerHp <= 0)
        {
            SceneManaged.Instance.SceneLoad((int)SceneManaged.SceneNameTags.GameOver);
        }
    }

    private void Start()
    {
        slider.value = playerHp;
    }
}
