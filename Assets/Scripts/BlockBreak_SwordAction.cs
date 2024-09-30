using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_SwordAction : MonoBehaviour
{
    public int Att;
    public GameObject spawner;
    public GameObject ground;
    public BlockBreak_PlayerAction Player;
    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    private float maxDistance;
    private int layerMask;
    private bool isAttack;
    public static bool isShield;
    AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        Att = 5;
        layerMask = 1 << 8;
        maxDistance = 2.0f;
        isAttack = false;
        isShield = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Total());
    }

    IEnumerator Total()
    {
        if(!BlockBreak_GameManager.GameOverFlag)
        {
            Defence();
        }
        if (!Player.DeadFlag)
        {
           Attack();
           StartCoroutine(SpecialPower());
        }
        yield return null;
    }

    private void Attack()
    {
        hit = Physics2D.Raycast(transform.position, transform.position, maxDistance, layerMask);
        Debug.DrawRay(transform.position, new Vector3(0, maxDistance, 0), new Color(0, 1, 0));
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(!isAttack)
            {
                StartCoroutine(Sword_Rotate(1));
                isAttack = true;
            }
            else
            {
                StartCoroutine(Sword_Rotate(-1));
                isAttack = false;
            }
            
            if (hit)
            {
                Debug.Log("공격성공");
                audioSource.Play();
                BlockBreak_GameManager.PowerGauge += 10f;
                hit.collider.GetComponent<BlockBreak_FallBrick>().Hp -= Att;
                if (hit.collider.GetComponent<BlockBreak_FallBrick>().Hp == 0)
                {
                    Destroy(hit.transform.gameObject);
                    BlockBreak_GameManager.TotalScore += hit.collider.GetComponent<BlockBreak_FallBrick>().Score;
                    BlockBreak_SpawnBrick.CountBrick--;
                }
            }
            
        }

        
        
        
        
    }

    IEnumerator Sword_Rotate(int dir)
    {
        
        for (int i = 0; i < 3; i++)
        {
            transform.Rotate(new Vector3(0, 0, 30 * dir));
            yield return new WaitForSeconds(0.001f);
        }
        
    }

    public void Defence()
    {
        hit2 = Physics2D.Raycast(transform.position, transform.position, maxDistance, layerMask);
        Debug.DrawRay(transform.position, new Vector3(0, maxDistance, 0), new Color(0, 1, 0));
        if (Input.GetKey(KeyCode.Z)&& !Player.DeadFlag)
        {
            BlockBreak_GameManager.ShieldGauge--;
            if (BlockBreak_GameManager.ShieldGauge <= 0)
            {
                BlockBreak_GameManager.ShieldGauge = 0;
                return;
            }
            isShield = true;
            if(ground.transform.position.y + 3 >= hit2.transform.position.y)
            {
                for (int i = 0; i < BlockBreak_SpawnBrick.CountBrick; i++)
                {
                    spawner.transform.GetChild(i).transform.position = new Vector3(0, spawner.transform.GetChild(i).transform.position.y + 8, 0);
                }
                return;
            }
            Debug.Log("방어중...");
            this.GetComponent<Collider2D>().isTrigger = false;
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            Debug.Log("방어 종료");
            isShield = false;
            this.GetComponent<Collider2D>().isTrigger = true;
        }

        if (Input.GetKeyDown(KeyCode.Z)&& ground.transform.position.y + 3 >= hit2.transform.position.y)
        {
            for (int i = 0; i < BlockBreak_SpawnBrick.CountBrick; i++)
            {
                spawner.transform.GetChild(i).transform.position = new Vector3(0, spawner.transform.GetChild(i).transform.position.y + 8, 0);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Z) && Player.DeadFlag)
        {
            for (int i = 0; i < BlockBreak_SpawnBrick.CountBrick; i++)
            {
                spawner.transform.GetChild(i).transform.position = new Vector3(0, spawner.transform.GetChild(i).transform.position.y+8, 0);
            }
            Player.DeadFlag = false;

        }
    }

    IEnumerator SpecialPower()
    {
        if (Input.GetKeyDown(KeyCode.Space) && BlockBreak_GameManager.PowerGauge >=100)
        {
            Debug.Log("궁극기 사용");
            Debug.DrawRay(transform.position, new Vector3(0, maxDistance, 0), new Color(0, 1, 0));
            while (BlockBreak_SpawnBrick.CountBrick != 0)
            {
                Debug.Log("궁극기 사용중");
                hit = Physics2D.Raycast(transform.position, transform.position, maxDistance, layerMask);
                Destroy(hit.collider.gameObject);
                Player.transform.position = new Vector3(0, Player.transform.position.y + 2f, 0);
                if (!isAttack)
                {
                    StartCoroutine(Sword_Rotate(1));
                    isAttack = true;
                }
                else
                {
                    StartCoroutine(Sword_Rotate(-1));
                    isAttack = false;
                }
                BlockBreak_GameManager.TotalScore += hit.collider.GetComponent<BlockBreak_FallBrick>().Score;
                yield return new WaitForSeconds(0.1f);
                BlockBreak_SpawnBrick.CountBrick--;
                maxDistance = maxDistance + 3f;
            }

            maxDistance = 2f;
            Player.transform.position = new Vector3(0,-2f,0);
            Player.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, -30f, 0), ForceMode2D.Impulse);
            BlockBreak_GameManager.PowerGauge = 0;
        }
    }



    
    
}
