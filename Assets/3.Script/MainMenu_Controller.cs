using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour
{
    [SerializeField] private Text Log;

    private Coroutine Log_co;

    public void SurverScene()
    {
        if (GameManager.instance.isLogin)
        {
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
        if (GameManager.instance.isLogin)
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
        while (LogTime < 0.4f)
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
