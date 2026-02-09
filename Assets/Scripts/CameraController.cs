using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    public bool AutoRotate = true; // 自動カメラワークフラグ

    public Vector3 center;// タイトル画面使用

    public PlayerManager player;
    public EnemyManager enemy;

    // Update is called once per frame
    void Update()
    {
        
        if (!AutoRotate) return;

        Vector3 playerPos = player.transform.position + Vector3.up * 1.0f;
        Vector3 enemyPos = enemy.transform.position + Vector3.up * 1.0f;

        center = (playerPos + enemyPos) / 2;// 回転の原点をプレイヤーと敵の中心に設定


        //Vector3 center = (player.transform.position + enemy.transform.position) * 0.5f;

        transform.RotateAround(center, Vector3.up, 0.1f);
        transform.LookAt(center);
        
        
    }
}
