using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Idle 7以上
 * Run 7以下
 * Attack 2以下
 */

public class EnemyManager : MonoBehaviour
{
    public Transform target; // プレイヤー
    NavMeshAgent agent; // プレイヤーを追尾するエネミー

    public Animator animator; // アニメーション取得

    public GameObject GameClearText; // UI
    public Collider weaponCollider; // コリジョン
    public BattleSceneDiretor battle;

    // エネミー情報関連
    public EnemyUIManager enemyUIManager;
    public EnemyAudioSouce enemyAudio;

    public int MaxHP = 100;
    int hp = 100;
    bool IsDie;
    bool IsDieAfter;

    // 行動遅延用
    float Delay = 2.0f;
    float DelayTimer;

    // テキストアニメーション用途
    float AnimTime = 1f;
    float textAnimTimer = 0f;
    bool IsAnimation = false;

    void Start()
    {
        // エネミー情報の初期化処理
        IsDie = false;
        IsDieAfter = false;
        hp = MaxHP;
        enemyUIManager.Init(this);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // エネミー自身取得
        agent.destination = target.position; // エージェントの追尾対象

        //enemyAudio = GetComponent<EnemyAudioSouce>();
        enemyAudio = GetComponentInChildren<EnemyAudioSouce>();

        HideColliderWeapon();
    }

    void Update()
    {
        if (!IsDieAfter) 

        agent.destination = target.position; // プレイヤーの位置を取得更新

        // ターゲットとの距離によって、Distanceにセットしアニメーション更新
        animator.SetFloat("Distance", agent.remainingDistance);

        GameClear();
        TextAnimation();
    }

    public void LookAtTarget()// ターゲットの方向を向く
    {
        transform.LookAt(target);
    }

    // 武器のコリジョンをオンオフ
    public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;
    }
    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;

        enemyAudio.EnemyAtackSE();
    }

    // 敵ダメージ処理
    void Damage(int damage)
    {
        if (IsDieAfter) return;

        hp -= damage;
        if (hp <= 0) // エネミーHPが0以下になったら
        {
            hp = 0;
            animator.SetTrigger("Die");// 死亡アニメーション
            animator.ResetTrigger("EnemyHurt");// ダメージ処理の停止
            
            IsDie = true;
        }

        enemyUIManager.UpdateHP(hp);// HPゲージ更新
        Debug.Log("敵の残りHP：" + hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();// ダメージをDamagerに限定
        if (damager != null)
        {
            // 当たり判定にぶつかったら
            animator.SetTrigger("EnemyHurt");
            Damage(damager.damage);
        }
    }

    void GameClear()
    {
        if (IsDieAfter) return;

        if (!IsDie) return;

        battle.DieAfterUI();
        // 死んだ直後のゲームオーバー処理
        
            DelayTimer += Time.deltaTime;// 遅延タイマーをセット

            if (DelayTimer >= Delay) // タイマーが設定した値に達したら
            {
                GameClearText.SetActive(true);// ゲームオーバーを有効化

                IsAnimation = true;// アニメーションフラグ
               
                battle.DieAfterUI();

                Destroy(gameObject, 2f); //2秒後オブジェクトを破棄
            }
        
    }

    void TextAnimation()
    {
        if (IsDieAfter) return;

        if (IsAnimation)
        {
            textAnimTimer += Time.deltaTime; // アニメタイマーセット

            float time = textAnimTimer / AnimTime; //取得した値を設定した値で割る
            time = Mathf.Clamp01(time);

            GameClearText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 2, time);

            GameClearText.transform.rotation =
                Quaternion.Lerp(
                    Quaternion.Euler(0, 0, 180), 
                    Quaternion.Euler(0, 0, 0),
                    time);

            if (time >= 1f) // アニメーションが終わると死後処理へ移行
            {
                IsAnimation = false;
                IsDieAfter = true;
            }
        }
    }
}
