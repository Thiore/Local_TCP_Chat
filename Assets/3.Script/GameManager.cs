using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] private GameObject Menu;
    [SerializeField] private Text Log;

    public bool isLogin;

    private Coroutine Log_co;

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isLogin = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Update()
    {
       
        if (!Menu.activeSelf&&Input.GetKeyDown(KeyCode.Escape)&&!SceneManager.GetActiveScene().name.Equals("SelectScene"))
        {
            Menu.SetActive(true);
        }
        else if (Menu.activeSelf&&Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(false);
        }

    }

    public void Continue()
    {
        Menu.SetActive(false);
    }

    public void ReturnSelect()
    {
        Menu.SetActive(false);
        SceneManager.LoadScene("SelectScene");
        
    }

    public void SurverScene()
    {
        if(isLogin)
        {
            Debug.Log("몇번째?");
            SceneManager.LoadScene("Server");
        }
        else
        {
            if (Log_co == null)
                Log_co = StartCoroutine(Please_Login());
        }
        
    }
    public void ClientScene()
    {
        if (isLogin)
        {
            SceneManager.LoadScene("Client");
        }
        else
        {
            if (Log_co == null)
                Log_co = StartCoroutine(Please_Login());
        }
    }
    private IEnumerator Please_Login()
    {
        Log.text = "로그인을 해주세요";
        float LogTime = 0;
        while(LogTime<0.4f)
        {
            LogTime += Time.deltaTime;
            Log.transform.Translate(Vector3.down * Time.deltaTime * 250f);
            yield return null;
        }
        LogTime = 0;
        while (LogTime < 0.4f)
        {
            LogTime += Time.deltaTime;
            
            yield return null;
        }
        LogTime = 0;
        while (LogTime < 0.4f)
        {
            LogTime += Time.deltaTime;
            Log.transform.Translate(Vector3.up * Time.deltaTime * 250f);
            yield return null;
        }
        Log.text = "";
        Log_co = null;
    }
}
