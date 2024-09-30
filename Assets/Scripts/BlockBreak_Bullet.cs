using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        float h = Random.Range(-1f, 2f);
        rigid.velocity = Vector2.right * 10 * h;
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
