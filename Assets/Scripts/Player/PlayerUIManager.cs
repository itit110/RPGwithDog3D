using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{
    public Slider hpSlider;

    public void Init(PlayerManager playerManager)
    {
        hpSlider.maxValue = playerManager.MaxHP;
        hpSlider.value = playerManager.MaxHP;
    }

    public void UpdateHP(int hp)
    {
        //hpSlider.value = hp;
        hpSlider.DOValue(hp, 1f);
    }

}
