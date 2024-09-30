using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlockBreak_GameManager : MonoBehaviour
{
    public Text Score;
    public Image PowerBar;
    public Image ShieldBar;

    public Text FinalScore;
    public Text NickName;
    public InputField Input;
    public Button MainButton;
    public Button SignUp;

    //BlockBreak_FallBrick bricks;
    public static bool isDead;
    public BlockBreak_PlayerAction player;
    public BlockBreak_DBManager DB;
    public Image[] Heart = new Image[3];
    public static float ShieldGauge;
    public static float PowerGauge;
    public float MaxPowerGauge;
    public float MaxShieldGauge;
    public static int TotalScore;
    public static bool GameOverFlag = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        TotalScore = 0;
        PowerGauge = 0;
        MaxPowerGauge = 100f;
        ShieldGauge = 100f;
        MaxShieldGauge = 100f;
        isDead = false;
        StartCoroutine(FillGauge());
        StartCoroutine(DeleteHeart());
        
    }

    // Update is called once per frame
    void Update()
    {
        AddScore();
        StayShieldGauge();
        //DeleteHeart();
        PowerBar.fillAmount = PowerGauge / MaxPowerGauge;
        ShieldBar.fillAmount = ShieldGauge / MaxShieldGauge;
    }
    

    private void AddScore()
    {
        Debug.Log(PowerGauge);
        Score.text = TotalScore.ToString();
    }

    private void StayShieldGauge()
    {
        if(ShieldGauge >= MaxShieldGauge)
        {
            ShieldGauge = 100;
        }
    }

    IEnumerator FillGauge()
    {
        while(true)
        {
            if (!BlockBreak_SwordAction.isShield)
            {
                ShieldGauge++;
            }
            if(PowerGauge >= 100)
            {
                PowerBar.color = new Color(1,0,0,1);
            }
            else
            {
                PowerBar.color = Color.white;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator DeleteHeart()
    {
        while(true)
        {
            if(isDead && !player.DeadFlag)
            {
                isDead = false;
            }
            if (player.DeadFlag && !isDead)
            {
                for (int i = 0; i < Heart.Length; i++)
                {
                    if(Heart[player.heart - i].gameObject.activeSelf)
                    {
                        Debug.Log(player.heart - i);
                        Heart[player.heart - i].gameObject.SetActive(false);
                        isDead = true;
                        break;
                    }
                    
                }

            }

            if(player.heart == 0)
            {
                GameOver();
            }
            yield return null;
        }
    }

    private void GameOver()
    {

        GameOverFlag = true;
        FinalScore.text = TotalScore.ToString();
        FinalScore.gameObject.SetActive(true);
        Input.gameObject.SetActive(true);
        SignUp.gameObject.SetActive(true);
        MainButton.gameObject.SetActive(true);




    }

    public void Addsign()
    {
        string name = NickName.text;
        int point = int.Parse(FinalScore.text);
        DB.DBConnectionCheck();
        DB.DatabaseInsert("Insert Into Ranking(NickName,Score) VALUES('" + name + "', " + point + ")");
        SceneManager.LoadScene("MainMenu");
    }

    public void ReMain()
    {
        SceneManager.LoadScene("MainMenu");

    }



    
}
