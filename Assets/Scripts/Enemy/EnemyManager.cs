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

    Animator animator; // アニメーション取得

    public GameObject GameClearText; // UI
    public Collider weaponCollider; // コリジョン

    // エネミー情報関連
    public EnemyUIManager enemyUIManager;
    public int MaxHP = 100;
    int hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        // エネミー情報の初期化処理
        hp = MaxHP;
        enemyUIManager.Init(this);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // エネミー自身取得
        agent.destination = target.position; // エージェントの追尾対象

        HideColliderWeapon();
    }

    void Update()
    {
        agent.destination = target.position; // プレイヤーの位置を取得更新

        // ターゲットとの距離によって、Distanceにセットしアニメーション更新
        animator.SetFloat("Distance", agent.remainingDistance);
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
    }

    // 敵ダメージ処理
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0) // エネミーHPが0以下になったら
        {
            hp = 0;
            animator.SetTrigger("Die");// 死亡アニメーション
            animator.ResetTrigger("EnemyHurt");// ダメージ処理の停止
            Destroy(gameObject, 2f); //2秒後オブジェクトを破棄

            //ゲームクリアテキストの表示
            GameClearText.SetActive(true);
            
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
}
