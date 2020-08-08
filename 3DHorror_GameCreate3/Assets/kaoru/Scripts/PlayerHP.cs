using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    //HP管理
    [SerializeField]int playerHp;
    [SerializeField] Slider slider;
    int hpSliderValue = 0;
    private void Start()
    {
        //hpSliderValue = (int)slider.value;
    }
    public void A(int Damage)
    {
        playerHp -= Damage;
        slider.value = playerHp;
        Debug.Log("PlayerHP=" + playerHp);
        
    }

}
