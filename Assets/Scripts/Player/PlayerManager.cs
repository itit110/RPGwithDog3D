using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody rb; // リジッドボディ
    Animator animator;
    float x, z; //移動入力用
    public float moveSpeed = 2.5f; // 移動値インスペクターから操作可能

    public Collider weaponCollider; // 武器のコリジョン
    public GameObject GameOverText;
    public Transform target; // エネミー情報
    // プレイヤー情報管理
    public PlayerUIManager playerUIManager;
    public int MaxHP = 100;
    int hp;
    bool IsDie;
    public bool IsAttack = false;

    // Start is called before the first frame update
    void Start()// Update前に一度実行
    {
        IsDie = false;
        hp = MaxHP;
        playerUIManager.Init(this); // HPゲージ初期化

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();// animatorコンポーネント取得

        HideColliderWeapon();
    }

    // Update is called once per frame
    void Update()// 毎フレームごとに更新
    {
        if (IsDie) return;

        // キーボード入力を使用して移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力：スペースキーで攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsAttack = true;
            animator.SetTrigger("Attack");
            // Debug.Log("攻撃");
        }

        UpdateRotaion();

    }

    void UpdateRotaion()
    {
        if (IsDie) return;

        // デフォルトでは入力方向へキャラクターが向く
        Vector3 direction = new Vector3(x, 0, z);
        

        if (IsAttack && target != null) // 攻撃中かつ、ターゲットがいる場合、敵を見る
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance <= 2f)// ターゲットとの距離が近い場合
            {
                direction = target.position - transform.position; // 敵の方向と自分の位置を引いた値を向きに代入
            }
        }
       
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void FixedUpdate()
    {
        if (IsDie) return;

        // 速度を設定
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        // Idle-> Run <-Idle
        animator.SetFloat("RunSpeed", rb.velocity.magnitude); // magnitude＝速度の大きさ
    }



    // 武器のコリジョン有効 / 無効
    public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;// 無効
    }
    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;// 有効
    }

    // プレイヤーダメージ処理
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            animator.SetTrigger("Die");
            IsDie = true; //HPでフラグをtrue

            animator.ResetTrigger("Hurt"); // ダメージリセット
            rb.isKinematic = true; // Die後の動きを止める

            GameOverText.SetActive(true);
        }

        playerUIManager.UpdateHP(hp);

        Debug.Log("プレイヤー残りHP：" + hp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDie) return; // 死んだ場合、ダメージ処理を行わない
       
        Damager damager = other.GetComponent<Damager>();// Damagerに限定
        if (damager != null)
        {
            // 当たり判定にぶつかったら
            //Debug.Log("プレイヤーにダメージ");
            animator.SetTrigger("Hurt");
            Damage(damager.damage);// Damager内で定義したダメージ
        }
    }
}
