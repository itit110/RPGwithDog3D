using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Idle 7以上
 * Run 7以下
 * Attack 2以下
 */

// プレイヤーを追尾する
public class EnemyManager : MonoBehaviour
{
    public Transform target; // プレイヤー
    NavMeshAgent agent;

    Animator animator; // アニメーション取得

    public GameObject GameClearText;

    public Collider weaponCollider; // コリジョン

    // エネミー情報関連
    public EnemyUIManager enemyUIManager;
    public int MaxHP = 100;
    int hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        hp = MaxHP;
        enemyUIManager.Init(this);

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // エネミー自身取得
        agent.destination = target.position;

        HideColliderWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;

        // ターゲットとの距離によって、Distanceにセットしアニメーション更新
        animator.SetFloat("Distance", agent.remainingDistance);

    }

    public void LookAtTarget()
    {

        transform.LookAt(target);
    }

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
        if (hp <= 0)
        {
            hp = 0;
            animator.SetTrigger("Die");
            Destroy(gameObject, 2f);
            GameClearText.SetActive(true); //ゲームクリア
            
        }
        enemyUIManager.UpdateHP(hp);
        Debug.Log("敵の残りHP：" + hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();// Damagerに限定
        if (damager != null)
        {
            // 当たり判定にぶつかったら
            //Debug.Log("敵にダメージ");
            animator.SetTrigger("EnemyHurt");
            Damage(damager.damage);
        }
    }
}
