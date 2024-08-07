using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//---------------------------
using System;//.net ���̺귯��
using System.Net;
using System.Net.Sockets;//������� �ϱ� ���� ���̺귯��
using System.IO;//�����͸� �а� ���� ���� ���̺귯��
using System.Threading;//��Ƽ�������� �ϱ� ���� ���̺귯��


public class TCP_Manager : MonoBehaviour
{
    public InputField IP_InputF;
    public InputField Port_InputF;

    [SerializeField] private Text Status;

    //��Ŷ -> .Net�������� Stream�̶�� �Ѵ�.
    private StreamReader reader;//������ �д� ��
    private StreamWriter writer;//������ ���� ��

    public InputField Message_Box;

    private Message_Pooling message;

    private Queue<string> Log = new Queue<string>();

    private void Status_Message()
    {//����ϱ� ���� �޼���
        if(Log.Count>0)
        {
            Status.text = Log.Dequeue();

        }
    }

    private void Update()
    {
            Status_Message();
    }

    #region Server
    public void Server_Open()
    {
        message = FindObjectOfType<Message_Pooling>();
        Thread thread = new Thread(Server_Connect);
        thread.IsBackground = true;
        thread.Start();

    }

    private void Server_Connect()
    {//������ �����ִ� �� -> ������ ����� ��
        //�帧�� ����ó���� �ϱ�����Try_Catch���
        try
        {
            //���� ������ ��
            TcpListener tcp = new TcpListener(IPAddress/*Ŭ�������ٴ� �ڷ����� �����*/.Parse(IP_InputF.text), int.Parse(Port_InputF.text));
            //TcpListener ��ü ���� �Ϸ�!
            tcp.Start();
            Log.Enqueue("Start_Server");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener�� client�� ������ �ɶ����� ��ٷȴٰ� ������ �Ǹ� -> Client�� �Ҵ�

            Log.Enqueue("Client ���� Ȯ��");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            while(client.Connected)
            {
                
                string readData = reader.ReadLine();
                message.Message(readData);
            }
            
        }
        catch(Exception e)
        {
            Log.Enqueue(e.Message);
        }
    }


    #endregion

    #region Client
    public void Client_Connect()
    {
        message = FindObjectOfType<Message_Pooling>();
        Log.Enqueue("Client_Connecting");
        Thread thread = new Thread(client_Connect);
        thread.IsBackground = true;
        thread.Start();
    }

    private void client_Connect()
    {//������ �����ϴ���
        try
        {
            TcpClient client = new TcpClient();


            //IPStartPoint = server
            //->client ->IPEndPoint
            IPEndPoint ipend = new IPEndPoint(IPAddress.Parse(IP_InputF.text), int.Parse(Port_InputF.text));

            client.Connect(ipend);
            Log.Enqueue("Client Server Connect Complete!");

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());

            writer.AutoFlush = true;

            while(client.Connected)
            {
                
                string readData = reader.ReadLine();
                message.Message(readData);
            }
        }
        catch(Exception e)
        {
            Log.Enqueue(e.Message);
        }
    }
    #endregion

    public void Sending_button()
    {
        //���� ���� �޼����� ���´ٸ� ���� ���� �޼����� MessageBox�� �־����
        if(Sending_Message(Message_Box.text))
        {
            message.Message($"{SQL_Manager.instance.info.User_Name} : {Message_Box.text}");
            Message_Box.text = string.Empty;
        }
    }
    private bool Sending_Message(string message)
    {
        if(writer != null)
        {
            writer.WriteLine($"{SQL_Manager.instance.info.User_Name} : {message}");
            return true;
        }
        else
        {
            Log.Enqueue("Writer null....");
            return false;
        }
    }



}
