using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    #region Public Variables
    public int maxHealth = 100;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject triggerArea;
    public GameObject triggerZone;
    public Collider2D hitBox;
    public GameObject player;
    public PlayerCombat playerCombat;
    public int player_health;
    #endregion

    #region Private Variables
    private int currentHealth;
    private Animator animator;
    private float distance;                 //Between our player and enemy
    private bool isAttack;
    private bool isCooldown;
    private float intTimer;
    #endregion

    private void Awake()
    {
        SelectTarget();
        animator = GetComponent<Animator>();
        intTimer = timer;

        player_health = playerCombat.playerCurrentHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isAttack)
        {
            Move();
        }

        if(!IsInsideLimit() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemiesLogic();
        }
    }

    void EnemiesLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance)
        {
            StopAttack();
        }
        else if(distance <= attackDistance && isCooldown == false)
        {
            Attack();
        }

        if (isCooldown)
        {
            CoolDown();
            animator.SetBool("Attacking", false);
        }
    }

    private void Move()
    {
        animator.SetBool("IsRunning", true);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))      //Check for current animationt isn't attack
        {
            Vector2 targetPos = new Vector2(target.position.x, transform.position.y);         //Take player position

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);    //Move enemy
        }
    }


    void Attack()
    {
        timer = intTimer;
        isAttack = true;

        if (isAttack == true)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("Attacking", true);

        }


    }

    public void AttackHitPlayer()       //Gọi ở 1 frame nhất định trong attack xem có đánh trúng player hay không
    {
        if(isAttack == true && CheckIfHitPlayer(hitBox, player) == true)
        {
            Debug.Log("hit");
            player_health -= 10;
            playerCombat.playerCurrentHealth -= 10;
        }
    }

    public bool CheckIfHitPlayer(Collider2D hitBox, GameObject player)
    {
            return hitBox.bounds.Contains(player.transform.position);
    }


    void StopAttack()
    {
        isCooldown = false;
        isAttack = false;
        animator.SetBool("Attacking", false);
    }

    public void TakeDamage(int damage)  
    {
        currentHealth -= damage;            //Biến damage lấy từ script PlayerCombat
        animator.SetTrigger("TakeDamage");          //Animation Take Damage

        if(currentHealth <= 0)
        {
            EnemiesDie();
        }
    }

    void EnemiesDie()
    {
        Debug.Log("Die!");
        animator.SetBool("IsDead", true);           //Animation Die

        GetComponent<Collider2D>().enabled = false;             //Tắt luôn collider để player có thể đi qua xác
        this.enabled = false;           //Tắt script này -> quái chết
    }

    void CoolDown()         //Tính toán thời gian dừng giữa các đòn tấn công của quái
    {
        timer -= Time.deltaTime;

        if(timer < 0 && isCooldown && isAttack)
        {
            isCooldown = false;
            timer = intTimer;           //Cài lại thời gian dừng
        }
    }

    void TriggerCooldown()      //Gọi trong frame attack
    {
        isCooldown = true;
    }

    private bool IsInsideLimit()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;     //Trả về true nếu quái đang trong vùng di chuyển
    }

    public void SelectTarget()
    {
        //Tính toán khoảng cách từ 2 điểm giới hạn tới quái
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;                 //nếu quái đang lệch phải thì mục tiêu đi tới là limit bên trái
        }
        else
        {
            target = rightLimit;                //Ngược lại!
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)        //Nếu player đang ở bên phải thì rotate để quay mặt
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
    }
}
