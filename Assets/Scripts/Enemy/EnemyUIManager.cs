using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyUIManager : MonoBehaviour
{
    public Slider hpSlider;

    public void Init(EnemyManager enemyManager)
    {
        hpSlider.maxValue = enemyManager.MaxHP;
        hpSlider.value = enemyManager.MaxHP;
    }

    public void UpdateHP(int hp)
    {
        //hpSlider.value = hp;
        hpSlider.DOValue(hp, 1f);// HPの減少を滑らかに表現
    }
}
