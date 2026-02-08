using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        hpSlider.value = hp;
    }
}
