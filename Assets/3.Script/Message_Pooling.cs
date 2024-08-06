using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Message_Pooling : MonoBehaviour
{
    [SerializeField] private Text[] message_Box;

    private string current_message = string.Empty;
    private string past_message;

    //대리자
    public Action<string> Message;
    public void Adding_Message(string message)
    {
        current_message = message;
    }

    private void Start()
    {
        message_Box = transform.GetComponentsInChildren<Text>();
        Message = Adding_Message;
        past_message = current_message;
    }

    private void Update()
    {
        if (past_message.Equals(current_message)) // 중복된 메세지인 경우 처리하지않고 넘기기
            return;
        ReadText(current_message);
        past_message = current_message;

    }

    private void ReadText(string message)
    {
        bool isInput = false;
        for(int i = 0; i < message_Box.Length;i++)
        {
            if(message_Box[i].text.Equals(""))
            {
                message_Box[i].text = message;
                isInput = true;
                break;
            }
        }
        if(!isInput)
        {
            for(int i = 1; i < message_Box.Length;i++)
            {
                message_Box[i - 1].text = message_Box[i].text;
            }
            message_Box[message_Box.Length - 1].text = message;
        }
    }

}
