using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour
{
    public Slider hpSlider; // プレイヤーHPの管理
    public Slider staminaSlider; // プレイヤーのスタミナゲージ管理

    public void Init(PlayerManager playerManager)
    {
        hpSlider.maxValue = playerManager.MaxHP;
        hpSlider.value = playerManager.MaxHP;

        staminaSlider.maxValue = playerManager.MaxStamina;
        staminaSlider.value = playerManager.MaxStamina;
    }

    public void UpdateHP(int hp)
    {
        //hpSlider.value = hp;
        hpSlider.DOValue(hp, 1f);
    }

    public void UpdateStamina(int stamina)
    {
        //hpSlider.value = hp;
        staminaSlider.DOValue(stamina, 1f);
    }

}
