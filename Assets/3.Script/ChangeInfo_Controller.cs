using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInfo_Controller : MonoBehaviour
{
    public Text ID;
    public InputField Password_InputF;
    public InputField re_EnterPassword_InputF;
    public InputField PhoneNum_InputF;

    [SerializeField] private Text Log;

    private void Start()
    {
        ID.text = SQL_Manager.instance.info.User_Name;
    }

    public void Change_Button()
    {
        
            if (Password_InputF.text.Equals(string.Empty))
            {
                Log.text = "비밀번호를 입력해주세요";
                return;
            }
            if (!Password_InputF.text.Equals(re_EnterPassword_InputF.text))
            {
                Log.text = "비밀번호를 다시 확인해주세요";
                return;
            }
            if (SQL_Manager.instance.Change(ID.text, Password_InputF.text, PhoneNum_InputF.text))
            {
                //로그인 성공
                gameObject.SetActive(false);
            }

    }
}
