using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Controller : MonoBehaviour
{
    public InputField ID_InputF;
    public InputField Password_InputF;

    [SerializeField] private GameObject ChangeButton;

    [SerializeField] private Text Log;

    private void Start()
    {
        if(GameManager.instance.isLogin)
        {
            ChangeButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Login_Button()
    {
        if(ID_InputF.text.Equals(string.Empty))
        {
            Log.text = "아이디를 입력해주세요";
            return;
        }
        if (Password_InputF.text.Equals(string.Empty))
        {
            Log.text = "비밀번호를 입력해주세요";
            return;
        }
        if (SQL_Manager.instance.Login(ID_InputF.text,Password_InputF.text))
        {
            //로그인 성공
            User_info info = SQL_Manager.instance.info;
            Debug.Log(info.User_Name + " | " + info.User_Password + " | " + info.User_PhoneNum);
            GameManager.instance.isLogin = true;
            ChangeButton.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            //로그인 실패
            Log.text = "아이디와 비밀번호를 확인해주세요";
        }
    }
}
