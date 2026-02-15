using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Rigidbody rb; // リジッドボディ
    Animator animator;// アニメーション取得
    float x, z; //移動入力用
    public float moveSpeed = 2.5f; // 移動値インスペクターから操作可能

    public Collider weaponCollider; // 武器のコリジョン
    public GameObject GameOverText;
    
    public BattleSceneDiretor battle;
    public Transform target; // エネミー情報取得
    public FloorDirector stage; //ステージ情報

    // プレイヤー情報管理
    PlayerAudioSouce playerAudio;
    public PlayerUIManager playerUIManager;
    public int MaxHP = 100;
    int hp;
    public int MaxStamina = 100;
    public int stamina;

    bool IsDie;
    bool IsDieAfter;
    public bool IsAttack = false;

    // 行動遅延用
    float Delay = 2.0f;
    float DelayTimer;
    bool IsDelay = false;

    // テキストアニメーション用途
    float AnimTime = 1f;
    float textAnimTimer = 0f;
    bool IsAnimation = false;

    void Start()// Update前に一度実行
    {
        // プレイヤー情報初期化
        IsDie = false;
        IsDieAfter = false;
        
        hp = MaxHP;
        stamina = MaxStamina;
        playerUIManager.Init(this); // HPゲージ初期化

        rb = GetComponent<Rigidbody>(); // 物理取得
        animator = GetComponent<Animator>();// animatorコンポーネント取得

        playerAudio = GetComponent<PlayerAudioSouce>();

        HideColliderWeapon(); // 武器のコリジョンを初期セットでオフ
    }

    void Update()// 毎フレームごとに更新
    {
        if (IsDieAfter) return;// 死んでたらreturn

        // キーボード入力を使用して移動
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        // 攻撃入力：スペースキーで攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (0 <= stamina)
            {
                Attack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))// Escでゲーム終了
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        // スタミナ管理
        Stamina();
        // 入力方向にプレイヤーを向ける
        UpdateRotaion();
        // ゲームオーバー判定
        GameOver();
        TextAnimation();
    }

    private void FixedUpdate()
    {
        if (IsDieAfter) return;

        // 速度を設定
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;

        // Idle-> Run <-Idle
        animator.SetFloat("RunSpeed", rb.velocity.magnitude); // magnitude＝速度の大きさ
    }

    void GameOver()
    {
        if (IsDieAfter) return;
        if (!IsDie) return;

        // 死んだ直後のゲームオーバー処理
        if (IsDie)
        {
            DelayTimer += Time.deltaTime;// 遅延タイマーをセット

            if (DelayTimer >= Delay) // タイマーが設定した値に達したら
            {
                GameOverText.SetActive(true);// ゲームオーバーを有効化

                IsAnimation = true;// アニメーションフラグ
                IsDelay = false; // 遅延オフ
                target.GetComponent<EnemyManager>().animator.SetTrigger("Idle");
                target.GetComponent<EnemyManager>().animator.SetFloat("Distance", 2.5f);
            }
           
        }
        
    }

    void TextAnimation()
    {
        if (!IsAnimation) return;

            textAnimTimer += Time.deltaTime; // アニメタイマーセット

            float time = textAnimTimer / AnimTime; //取得した値を設定した値で割る
            time = Mathf.Clamp01(time);

            GameOverText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1f, time);

            GameOverText.transform.rotation =
                Quaternion.Lerp(
                    Quaternion.Euler(0, 0, 180),
                    Quaternion.Euler(0, 0, 0),
                    time);

        if (time >= 1f)
        {
            IsAnimation = false;
            IsDieAfter = true;
            battle.DieAfterUI();
        }
    }

    void Attack()
    {
        if (stamina <= 1) { IsDelay = true; }

        if (stamina >= 10)// スタミナが0以上であれば攻撃可能
        {
                stamina -= 10;// 攻撃ごとにスタミナが20減少
            
        if(stamina <= 0) // スタミナが0以下の場合
        {
        stamina = 0;
        IsDelay = true;// 遅延フラグを真
        DelayTimer = 0f;// 遅延タイマーをセット
        }

            playerUIManager.UpdateStamina(stamina);
            IsAttack = true;
            animator.SetTrigger("Attack");
        }
    }

    void Stamina()
    {
        if (stamina >= MaxStamina) return;// スタミナがMaxを超えれば処理をしない

        if(IsDelay)// 遅延フラグが真の時の処理
        {
            DelayTimer += Time.deltaTime;// 経過時間を取得

            // 設定した遅延の値を超えた場合に遅延フラグを偽
            if (DelayTimer >= Delay)
            {
                IsDelay = false;
            }
            else
            {
                return;// 達していなければ処理をしない
            }
        }

        if (IsIdle())// アイドル時のみスタミナ回復
        {
            stamina++;
        }
            playerUIManager.UpdateStamina(stamina);
        
        
    }

    private bool IsIdle()
    {
        return rb.velocity.magnitude < 0.01 && !IsAttack;// 移動していないかつ攻撃していなければアイドル時の真を返す
        
    }

    void UpdateRotaion()
    {
        if (IsDieAfter) return;

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
            transform.rotation = Quaternion.LookRotation(direction);// ブレ防止
        }
    }

    // 武器のコリジョン有効 / 無効
    public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;// 無効
    }
    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;// 有効

        if(playerAudio != null || IsAttack)
        {
            playerAudio.PlayerAtackSE();
        }
    }

    // プレイヤーダメージ処理
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)// HPが0以下になった場合
        {
            hp = 0;
            animator.ResetTrigger("Hurt"); // ダメージリセット
            animator.SetTrigger("Die");// 死亡アニメーション
            IsDie = true; //HPでフラグをtrue

            rb.isKinematic = true; // Die後の動きを止める
        }

        playerUIManager.UpdateHP(hp);//HP減少のUI処理
        Debug.Log("プレイヤー残りHP：" + hp);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsDie) return; // 死んだ場合、ダメージ処理を行わない
       
        Damager damager = other.GetComponent<Damager>();// Damagerに限定
        if (damager != null)// 当たり判定にぶつかったら
        {
            //Debug.Log("プレイヤーにダメージ");
            animator.SetTrigger("Hurt");
            Damage(damager.damage);// Damager内で定義したダメージ
        }
    }
        

}
