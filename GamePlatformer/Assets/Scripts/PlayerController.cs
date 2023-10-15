using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = true;
    bool isDead = false;
    int idMove = 0; //0 untuk karakter begerak, 1 bergerak ke kiri, 2 bergerak ke kanan
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Jump "+isJump);
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }
        Move();
        Dead();

        void OnCollisionStay2D(Collision2D collision)
        {
            //kondisi ketika menyentuh tanah, memastikan animasi tidak dalam keadaan lompat dan tidak ada perintah untuk gerak, hasilnya animasi IDLE
            if(isJump)
            {
                anim.ResetTrigger("jump");
                if(idMove == 0) anim.SetTrigger("idle");
                isJump = false;
            }
        }

       void OnCollisionExit2D(Collision2D collision)
        {
            // ketika karakter tidak menyentuh tanah, Animasi loncat, Run, Idle
            anim.ResetTrigger("jump");
            anim.ResetTrigger("run");
            anim.ResetTrigger("idle");
            isJump = true;
        }

        void MoveRight()
        {
            idMove = 1;
        }

        void MoveLeft()
        {
            idMove = 2;
        }

       void Move()
        {
            if(idMove == 1 && !isDead)
            {
                //pergerakan karakter ke kanan
                if (!isJump) anim.SetTrigger("run");
                transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if(idMove == 2 && !isDead)
            {
                //pergerakan karakter ke kiri
                if (!isJump) anim.SetTrigger("run");
                transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        void Jump()
        {
            if(!isJump)
            {
               gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300f);
            }
        }

        void OnTriggerEnter2D(Collider2D collision) 
        { 
            if (collision.transform.tag.Equals("Coin"))
            {
                //Data.score += 15; 
                Destroy(collision.gameObject);
            }
        }

        void Idle()
        { // kondisi ketika idle/diam 
            if (!isJump) 
            { 
                anim.ResetTrigger("jump"); 
                anim.ResetTrigger("run"); 
                anim.SetTrigger("idle"); 
            } 
            idMove = 0;
        }

        void Dead()
        { 
            if (!isDead) 
            { 
                if (transform.position.y < -10f) 
                { 
                    // kondisi ketika jatuh 
                    isDead = true; 
                }
            } 
        }
    }
}
