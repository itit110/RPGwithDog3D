using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    public Vector3 center;
    public bool AutoRotate = true; // 自動カメラワークフラグ

    public Transform player;
    public Transform enemy;

    // Update is called once per frame
    void Update()
    {
        if (!AutoRotate) return;

        Vector3 playerPos = player.transform.position + Vector3.up;
        Vector3 enemyPos = enemy.transform.position + Vector3.up;

        center = (playerPos + enemyPos) / 2;// 回転の原点をプレイヤーと敵の中心に設定

        transform.RotateAround(center, Vector3.up, 0.1f);// 原点を中心として自動で回転する
        transform.LookAt(center);// キャラクターは中心を見る
    }
}
