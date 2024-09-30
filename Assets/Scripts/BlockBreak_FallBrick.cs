using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_FallBrick : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;
    public float gravityPower;
    public int Hp;
    public int Score;
    public GameObject bullet;
    void Awake()
    {
        gravityPower = 5f;
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = Vector2.down * gravityPower;
    }

    void OnDestroy()
    {
        makebullet();
    }

    private void makebullet()
    {
        int num = Random.Range(10, 31);
        GameObject[] bullets = new GameObject[num];
        for (int i = 0; i < num; i++)
        {
            bullets[i] = bullet;
        }
        for (int i = 0; i < num; i++)
        {
            float X = Random.Range(-5, 5);
            float Y = Random.Range(0, 3);
            Instantiate(bullets[i], new Vector3(transform.position.x+X,transform.position.y+Y,0), transform.rotation);
        }

    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }


}
