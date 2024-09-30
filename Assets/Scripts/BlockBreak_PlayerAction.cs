using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_PlayerAction : MonoBehaviour
{
    public int heart;
    int playerLayer, brickLayer;
    public float Gravity;
    public int JumpPower;
    public bool isGround;
    public bool isJump;
    public bool isAttack;
    public bool isDefence;
    public bool DeadFlag;
    
    public float movement;
    private float maxDistance = 2f;
    private int layerMask;

    public Camera Camera;
    private RaycastHit2D hit;
    public BlockBreak_SwordAction Sword;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        brickLayer = LayerMask.NameToLayer("Brick");
        Gravity = 2f;
        rigid = GetComponent<Rigidbody2D>();
        JumpPower = 30;
        movement = 5;
        isGround = true;
        isJump = true;
        isAttack = false;
        isDefence = false;
        DeadFlag = false;
        
        heart = 3;
        layerMask = 1 << 8;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DeadFlag)
        {
            PlayerMove();
        }
        CameraMove();
        PlayerGhost();
        PlayerDead();
    }

    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isGround && transform.position.x > -5 && isJump)
            {
                transform.position = new Vector3(transform.position.x - movement, -3.7f, 0);
                isGround = false;
                StartCoroutine(waitingGround());
            }

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isGround && transform.position.x < 5 && isJump)
            {
                transform.position = new Vector3(transform.position.x + movement, -3.7f, 0);
                isGround = false;
                StartCoroutine(waitingGround());
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && isJump)
        {
            rigid.AddForce(new Vector3(0, JumpPower, 0), ForceMode2D.Impulse);
            isJump = false;

        }


    }

    private void CameraMove()
    {
        if(isJump)
        {
            Camera.transform.position = new Vector3(0, 4, -10);
        }
        else if(Camera.transform.position.y<transform.position.y)
        {
            Camera.transform.position = new Vector3(0, transform.position.y, -10); 
        }
        else
        {
            Camera.transform.position = new Vector3(0, transform.position.y, -10);
        }
        
    }
    void PlayerGhost()
    {
        if(transform.position.y <= -3.6)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, brickLayer, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayer, brickLayer, false);
        }

    }

    void PlayerDead()
    {
        
        hit = Physics2D.Raycast(transform.position, transform.position, maxDistance, layerMask);
        Debug.DrawRay(transform.position, new Vector3(0, maxDistance, 0), new Color(0, 1, 0));
        
        if(hit)
        {
            if (transform.position.y + 3 <= hit.transform.position.y)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Sword.Defence();
                    BlockBreak_GameManager.isDead = false;
                }
            }

            if (hit.transform.position.y <= -3.1 && !DeadFlag)
            {
                DeadFlag = true;
                heart--;
            }
        }

        if(heart == 0)
        {
            Debug.Log("죽음");
        }
    }



    IEnumerator waitingGround()
    {
        yield return new WaitForSeconds(0.1f);
        isGround = true;
    }
    


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("재충전");
            isJump = true;
        }

        if (collision.collider.CompareTag("Brick"))
        {
            Debug.Log("블럭에 닿음");
            
        }


    }
}
