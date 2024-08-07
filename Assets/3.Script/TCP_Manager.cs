using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//---------------------------
using System;//.net 라이브러리
using System.Net;
using System.Net.Sockets;//소켓통신 하기 위한 라이브러리
using System.IO;//데이터를 읽고 쓰기 위한 라이브러리
using System.Threading;//멀티쓰래딩을 하기 위한 라이브러리


public class TCP_Manager : MonoBehaviour
{
    public InputField IP_InputF;
    public InputField Port_InputF;

    [SerializeField] private Text Status;

    //패킷 -> .Net기준으로 Stream이라고 한다.
    private StreamReader reader;//데이터 읽는 놈
    private StreamWriter writer;//데이터 쓰는 놈

    public InputField Message_Box;

    private Message_Pooling message;

    private Queue<string> Log = new Queue<string>();

    private void Status_Message()
    {//출력하기 위한 메서드
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
    {//서버를 열어주는 쪽 -> 서버를 만드는 쪽
        //흐름에 예외처리를 하기위해Try_Catch사용
        try
        {
            //서버 열리는 쪽
            TcpListener tcp = new TcpListener(IPAddress/*클래스보다는 자료형에 가까움*/.Parse(IP_InputF.text), int.Parse(Port_InputF.text));
            //TcpListener 객체 생성 완료!
            tcp.Start();
            Log.Enqueue("Start_Server");

            TcpClient client = tcp.AcceptTcpClient();
            //TcpListener가 client가 연결이 될때까지 기다렸다가 연결이 되면 -> Client에 할당

            Log.Enqueue("Client 연결 확인");

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
    {//서버에 접근하는쪽
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
        //만약 내가 메세지를 보냈다면 내가 보낸 메세지도 MessageBox에 넣어야함
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
