using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 9f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheckRadius = 0.25f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //Kiểm tra xem có đang chạm đất không và trả về true hoặc false
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Kiểm tra xem có đang ở trên mặt đất hay không

        if (isTouchingGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("isJumping", true);
              
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }



    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Di chuyển nhân vật
        float moveHorizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal)); //Đổi animation sang Walking

        if(moveHorizontal < 0)          //Nếu player di chuyển sang trái thì flip.
        {
            spriteRenderer.flipX = true;
        }
        else if (moveHorizontal > 0)        //Di chuyển sang phải tắt flipX.
        {
            spriteRenderer.flipX = false;
        }

    }

}
