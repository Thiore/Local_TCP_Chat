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
            Log.text = "���̵� �Է����ּ���";
            return;
        }
        if(SQL_Manager.instance.Check_Name(ID_InputF.text).Equals(0))
        {
            Log.text = "���� ����";
        }
        else if (SQL_Manager.instance.Check_Name(ID_InputF.text).Equals(1))
        {
            Log.text = "������ ����ڰ� �����մϴ�.";
        }
        else
        {
            Log.text = "��밡���� �̸��Դϴ�.";
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
                Log.text = "��й�ȣ�� �Է����ּ���";
                return;
            }
            if(!Password_InputF.text.Equals(re_EnterPassword_InputF.text))
            {
                Log.text = "��й�ȣ�� �ٽ� Ȯ�����ּ���";
                return;
            }
            if (SQL_Manager.instance.Join(ID_InputF.text, Password_InputF.text, PhoneNum_InputF.text))
            {
                //�α��� ����
                gameObject.SetActive(false);
            }
            else
            {
                //�α��� ����
                Log.text = "���̵�� ��й�ȣ�� Ȯ�����ּ���";
            }
        }
        else
        {
            Log.text = "�̸��� �ٽ� �Է��ϼ���";
        }
        
    }
}
