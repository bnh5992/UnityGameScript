using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlockBreak_MainGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField IDField;
    public InputField PWField;
    public InputField createIDField;
    public InputField createPWField;
    public Button CreateButton;
    public Button LoginButton;
    public Button RigisterButton;
    public Text createID;
    public Text createPassWord;
    public Text ID;
    public Text PassWord;
    public BlockBreak_DBManager db;


    // Update is called once per frame
    public void GameStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RankingScene()
    {
        SceneManager.LoadScene("RankingScene");
    }

    

    public void Rigister()
    {
        IDField.gameObject.SetActive(false);
        PWField.gameObject.SetActive(false);
        createIDField.gameObject.SetActive(true);
        createPWField.gameObject.SetActive(true);
        LoginButton.gameObject.SetActive(false);
        RigisterButton.gameObject.SetActive(false);
        CreateButton.gameObject.SetActive(true);
    }

    public bool DoubleCheck(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(db.GetDBFilePath());
        dbConnection.Open(); // db열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; //쿼리입력
        IDataReader dataReader = dbCommand.ExecuteReader(); //쿼리 실행
        while (dataReader.Read())
        {
            if (dataReader.GetString(0) == ID.text)
            {
                return true;
            }
        }
        return false;
    }

    public bool LoginCheck(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(db.GetDBFilePath());
        dbConnection.Open(); // db열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query; //쿼리입력
        IDataReader dataReader = dbCommand.ExecuteReader(); //쿼리 실행
        while (dataReader.Read())
        {
            if (dataReader.GetString(0) == ID.text && dataReader.GetString(1) == PassWord.text)
            {
                return true;
            }
        }
        return false;
    }

    public void Create()
    {

        if (createID.text.Length <= 1)
        {
            Debug.Log(createID.text.Length);
            Debug.Log("최소 1개 이상의 이름이 필요합니다.");
            return;
        }
        if(DoubleCheck("Select * From user_Info"))
        {
            Debug.Log("중복입니다");
            return;
        }
        string name = createID.text;
        string password = createPassWord.text;
        db.DatabaseInsert("Insert Into user_Info(id, password) VALUES('" + name + "', '"+ password + "')");
        Debug.Log("등록 완료");
        IDField.gameObject.SetActive(true);
        PWField.gameObject.SetActive(true);
        createIDField.gameObject.SetActive(false);
        createPWField.gameObject.SetActive(false);
        LoginButton.gameObject.SetActive(true);
        RigisterButton.gameObject.SetActive(true);
        CreateButton.gameObject.SetActive(false);

    }

    public void Login()
    {
        if(LoginCheck("Select * From user_Info"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("잘못된 입력입니다");
        }
    }


    
}
