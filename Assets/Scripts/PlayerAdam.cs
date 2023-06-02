using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdam : MonoBehaviour
{
    public float maxSpeed = 4;
    public float jumpForce = 400;

    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
    }

    
    void Update()
    {
        //valida se empty object GroundCheck esta em contato com o layer do chao
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if(Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if(!onGround) //trava vertical durante pulo
                z = 0;

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed );

            if(h > 0 && !facingRight) 
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if(jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    void Flip ()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
