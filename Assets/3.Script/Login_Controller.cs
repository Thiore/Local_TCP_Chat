using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Controller : MonoBehaviour
{
    public InputField ID_InputF;
    public InputField Password_InputF;

    [SerializeField] private Text Log;

    public void Login_Button()
    {
        if(ID_InputF.text.Equals(string.Empty) || Password_InputF.text.Equals(string.Empty))
        {
            Log.text = "아이디와 비밀번호를 입력해주세요";
            return;
        }
        if(SQL_Manager.instance.Login(ID_InputF.text,Password_InputF.text))
        {
            //로그인 성공
            User_info info = SQL_Manager.instance.info;
            Debug.Log(info.User_Name + " | " + info.User_Password + " | " + info.User_PhoneNum);
            gameObject.SetActive(false);
        }
        else
        {
            //로그인 실패
            Log.text = "아이디와 비밀번호를 확인해주세요";
        }
    }
}
