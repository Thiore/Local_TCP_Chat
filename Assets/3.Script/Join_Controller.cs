using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Join_Controller : MonoBehaviour
{
    public InputField ID_InputF;
    public InputField Password_InputF;
    public InputField re_EnterPassword_InputF;
    public InputField PhoneNum_InputF;

    [SerializeField]private GameObject CheckButton;

    [SerializeField] private Text Log;

    private bool isCheck_Name = false;

    public void Check_Button()
    {
        if (ID_InputF.text.Equals(string.Empty))
        {
            Log.text = "아이디를 입력해주세요";
            return;
        }
        if(SQL_Manager.instance.Check_Name(ID_InputF.text).Equals(0))
        {
            Log.text = "연결 실패";
        }
        else if (SQL_Manager.instance.Check_Name(ID_InputF.text).Equals(1))
        {
            Log.text = "동일한 사용자가 존재합니다.";
        }
        else
        {
            Log.text = "사용가능한 이름입니다.";
            CheckButton.SetActive(false);
            isCheck_Name = true;
        }


    }

    public void Join_Button()
    {
        if(isCheck_Name)
        {
            if (Password_InputF.text.Equals(string.Empty))
            {
                Log.text = "비밀번호를 입력해주세요";
                return;
            }
            if(!Password_InputF.text.Equals(re_EnterPassword_InputF.text))
            {
                Log.text = "비밀번호를 다시 확인해주세요";
                return;
            }
            if (SQL_Manager.instance.Join(ID_InputF.text, Password_InputF.text, PhoneNum_InputF.text))
            {
                //로그인 성공
                gameObject.SetActive(false);
            }
            else
            {
                //로그인 실패
                Log.text = "아이디와 비밀번호를 확인해주세요";
            }
        }
        else
        {
            Log.text = "이름을 다시 입력하세요";
        }
        
    }
}
