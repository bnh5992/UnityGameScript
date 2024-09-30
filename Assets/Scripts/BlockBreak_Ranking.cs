using UnityEngine;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockBreak_Ranking : MonoBehaviour
{
    // Start is called before the first frame update
    public Text[] Nickname;
    public Text[] Score;
    //BlockBreak_DBManager db;
    public GameObject Script;
    void Start()
    {
        //db.DataGet("Select * From Ranking"); 금지
        Script.GetComponent<BlockBreak_DBManager>().DataGet("Select * From Ranking order by Score Desc");
    }

    // Update is called once per frame
    

    public void BackScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
