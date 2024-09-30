using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_SpawnBrick : MonoBehaviour
{
    public static int CountBrick;
    public int RandomBrick;
    public Transform spawnPosition;
    public GameObject[] SpawnBox = new GameObject[4];
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        CountBrick = 10;
        SpawnBricks();
    }

    // Update is called once per frame
    void Update()
    {
        if (CountBrick == 0)
        {
            SpawnBricks();
        }
    }

    private void SpawnBricks() //블록을 랜덤하게 다른 종류로 생성한다.
    {
        RandomBrick = Random.Range(5, 10);
        CountBrick = RandomBrick;
        GameObject[] objects = new GameObject[RandomBrick];
        for (int i = 0; i < RandomBrick; i++)
        {
            int SelectBrick = Random.Range(0, 4);
            objects[i] = SpawnBox[SelectBrick];
            Instantiate(objects[i], spawnPosition.position, spawnPosition.rotation).transform.parent = parent.transform;
        }
    }
}
