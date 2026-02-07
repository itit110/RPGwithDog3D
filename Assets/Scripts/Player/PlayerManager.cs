using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody rb; // リジッドボディ
    Animator animator; 

    float x, z; //移動入力用
    public float moveSpeed = 2.5f; // 移動値インスペクターから操作可能

    public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()// Update前に一度実行
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();// animatorコンポーネント取得

        HideColliderWeapon();
    }

    // Update is called once per frame
    void Update()// 毎フレームごとに更新
    {
        // キーボード入力を使用して移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力：スペースキーで攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("攻撃");
            animator.SetTrigger("Attack");
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 direction = transform.position + new Vector3(x, 0, z) * moveSpeed;
        transform.LookAt(direction);

        // 速度を設定
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        // Idle-> Run <-Idle
        animator.SetFloat("RunSpeed", rb.velocity.magnitude); // magnitude＝速度の大きさ

    }

    // 武器の有効無効
    public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;
    }
    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent<Damager>();// Damagerに限定
        if (damager != null)
        {
            // 当たり判定にぶつかったら
            //Debug.Log("プレイヤーにダメージ");
            animator.SetTrigger("Hurt");
        }
    }
}
