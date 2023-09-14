using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttack : MonoBehaviour
{
    #region Public Variables
    public int damge = 10;
    private Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
        }
    }
}
