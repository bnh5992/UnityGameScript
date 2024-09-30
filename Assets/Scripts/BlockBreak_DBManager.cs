using UnityEngine;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


public class BlockBreak_DBManager : MonoBehaviour
{
    //private static BlockBreak_DBManager instance = null;
    static IDbConnection dbConnection;
    static IDbCommand dbCommand;
    public static IDataReader dataReader;
    public BlockBreak_Ranking rank;



/*    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(DBCreate());
    }*/
    // Start is called before the first frame update
    void Awake()
    {
        DBConnectionCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DBCreate()
    {
        string filepath = string.Empty;
        filepath = Application.dataPath + "/Ranking.db";
        if (!File.Exists(filepath))
        {
            File.Copy(Application.streamingAssetsPath + "/Ranking.db", filepath);
        }
        yield return new WaitForSeconds(0.1f);
        print("DB 생성 완료");
        
    }

    public string GetDBFilePath()
    {
        string str = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            str = "URI=file:" + Application.persistentDataPath + "/Ranking.db";
        }
        else
        {
            str = "URI=file:" + Application.streamingAssetsPath + "/Ranking.db";
        }
        return str;
    }

    public void DBConnectionCheck()
    {
        try
        {
            dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open)
            {
                print("DB연결 성공");
            }
            else
            {
                print("연결실패[ERROR]");
            }
        }
        catch (Exception e)
        {
            print(e);
        }
    }
    public void DataBaseRead(string query)
    {
        int count = 0;
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open(); // db열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; //쿼리입력
        IDataReader dataReader = dbCommand.ExecuteReader(); //쿼리 실행
        while (dataReader.Read())
        {
            if (count == 3) break;
            Debug.Log(dataReader.GetString(0) + "," + dataReader.GetString(1));
            //0,1번 필드 읽기
            count++;
        }
        dataReader.Dispose(); //생성순서과 반대로 닫아줍니다.
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DataGet(string query)
    {

        int count = 0;
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open(); // db열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; //쿼리입력
        IDataReader dataReader = dbCommand.ExecuteReader(); //쿼리 실행
        while (dataReader.Read())
        {
            if (count == 3) break;
            rank.Nickname[count].text = dataReader.GetString(0);
            rank.Score[count].text = dataReader.GetString(1);
            //0,1번 필드 읽기
            count++;
        }
        dataReader.Dispose(); //생성순서과 반대로 닫아줍니다.
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }

    public void DatabaseInsert(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open(); // db열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; //쿼리입력
        dbCommand.ExecuteNonQuery(); // 쿼리 실행

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
    }
}
