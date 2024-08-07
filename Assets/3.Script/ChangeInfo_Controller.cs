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
                Log.text = "��й�ȣ�� �Է����ּ���";
                return;
            }
            if (!Password_InputF.text.Equals(re_EnterPassword_InputF.text))
            {
                Log.text = "��й�ȣ�� �ٽ� Ȯ�����ּ���";
                return;
            }
            if (SQL_Manager.instance.Change(ID.text, Password_InputF.text, PhoneNum_InputF.text))
            {
                //�α��� ����
                gameObject.SetActive(false);
            }

    }
}
