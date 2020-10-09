using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    //HP管理
    [SerializeField]int playerHp;
    [SerializeField] Slider slider;
    
    public void A(int Damage)
    {
        playerHp -= Damage;
        slider.value = playerHp;
        SceneManaged.Instance.Test(playerHp);//test
    }

    private void Start()
    {
        slider.value = playerHp;
    }
}
