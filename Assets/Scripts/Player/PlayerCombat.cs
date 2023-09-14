using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.45f;
    public LayerMask enemyLayer;
    public int attackDamage = 25;
    public float attackSpeed = 1.2f;
    public int playerCurrentHealth;         //Máu hiện tại
    #endregion

    #region Private Variables
    private int playerHealth;       //Máu tối đa
    private float nextAttackTime = 0f;
    #endregion

    private void Awake()
    {
        playerHealth = 100;
        playerCurrentHealth = playerHealth;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))        //Nhấn chuột trái thực hiện đòn tấn công
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;          //Tính toán thời gian để tấn công lần tiếp theo
            }
        }


    }

    void Attack()
    {
        animator.SetTrigger("Attacking");       //Attack animation

        // Kiểm tra mọi thứ trong tầm đánh xem có kẻ địch nào không
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemiesController>().TakeDamage(attackDamage);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
