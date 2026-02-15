using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneCameraController : MonoBehaviour
{
    // カメラの原点とプレイヤー位置参照用
    public PlayerManager player;
    public EnemyManager enemy;
    public Vector3 center;// 原点座標

    // プレイヤー追従カメラ設定
    [Header("追従設定")]
    [SerializeField] private Vector3 offset = new Vector3(0, -5.0f, -4.0f);// オフセット位置
    [SerializeField] private float smoothSpeed = 1.0f;// カメラ移動速度

    [Header("スタート演出")]
    [SerializeField] private bool IsIntroFinished = false; // プレイヤー追従移行
    [SerializeField] private float Delay = 5.0f;// 遅延用

    [Header("UI演出")]
    
    [SerializeField] private GameObject DuelText;
    [SerializeField] private Vector3 StartPos = new Vector3(0, 500, 0);// テキストのスタート位置
    //[SerializeField] private float startScale = 2.0f;
    Vector3 loacalPos;// Sceneで設定したテキスト位置

    float timer = 0;// 経過時間保存用

    void Start()
    {
        loacalPos = DuelText.transform.localPosition;
        DuelText.transform.localPosition = loacalPos + StartPos;
        //DuelText.transform.localScale = Vector3.one * startScale;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsIntroFinished)
        {
            timer += Time.deltaTime;
            if (timer > Delay) IsIntroFinished = true;

            RotationCamera();
            
            float progress = timer / Delay;
            
            DuelText.transform.localPosition = Vector3.Lerp(
                loacalPos + StartPos,
                loacalPos, progress );
            
            /*
            DuelText.transform.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one / 1.5f, progress );
            */
            
        }
        else
        {
            
            MoveCamera(smoothSpeed * 5f);
            Vector3 LookAtPos = player.transform.position + Vector3.up * 1.0f;
            transform.LookAt(LookAtPos);
        }
    }

    void MoveCamera(float speed)
    {
        offset.y = 0.08f;
        Vector3 playerPosition = player.transform.position + offset;
        
        transform.position = Vector3.Lerp(transform.position, playerPosition, speed * Time.deltaTime);
        DuelText.SetActive(false);
    }

    void RotationCamera()
    {
        Vector3 playerPos = player.transform.position + Vector3.up * 1.0f;
        Vector3 enemyPos = enemy.transform.position + Vector3.up * -2.5f;

        center = (playerPos + enemyPos) / 2;

        transform.RotateAround(center, Vector3.up, 0.1f);
        transform.LookAt(center);
    }
}
