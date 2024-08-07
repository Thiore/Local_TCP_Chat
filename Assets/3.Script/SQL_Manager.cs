using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;

public class User_info
{//Table 데이터 자료형으로 만들어 놓음
    public string User_Name { get; private set; }
    public string User_Password { get; private set; }
    public string User_PhoneNum { get; private set; }

    public User_info(string name, string password, string phone)
    {
        User_Name = name;
        User_Password = password;
        User_PhoneNum = phone;
    }
}
public class SQL_Manager : MonoBehaviour
{
    //교수님의 개인적인 성향이 반영됨
    public User_info info;
    public MySqlConnection connection; // 연결
    public MySqlDataReader reader; // 데이터를 직접적으로 읽어오는 자료형? 클래스?

    [SerializeField] private string DB_Path = string.Empty;

    public static SQL_Manager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            info = null;
            
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DB_Path = Application.dataPath + "/Database";
        string serverinfo = Server_set(DB_Path);
        try
        {
            if (serverinfo.Equals(string.Empty))
            {
                Debug.Log("server info = null");
                return;
            }
            connection = new MySqlConnection(serverinfo);
            connection.Open();
            Debug.Log("DB Open Connection Complete");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string Server_set(string path)
    {//파일이 존재한다면 Json을 풀어서 전달
        if(!File.Exists(path))
        {
            Directory.CreateDirectory(path);//경로 파일

        }
        string JsonString = File.ReadAllText(path + "/config.json");
        JsonData itemData = JsonMapper.ToObject(JsonString);
        string serverInfo = $"Server = {itemData[0]["IP"]};" +
                            $"Database = {itemData[0]["TableName"]};" +
                            $"Uid = {itemData[0]["ID"]};" +
                            $"Pwd = {itemData[0]["PW"]};" +
                            $"Port = {itemData[0]["PORT"]};" +
                            "CharSet = utf8";

        return serverInfo;
    }

    private bool Connection_Check(MySqlConnection connection)
    {
        if(connection.State != System.Data.ConnectionState.Open)
        {
            connection.Open();
            if(connection.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }

    public bool Login(string ID, string Password)
    {
        //직접적으로 DB에서 데이터를 가지고 오는 메서드
        /*
         만약에 조회되는 데이터가 없다면 반환값 False
         조회가 되는 데이터가 있다면 True를 던지긴 하는데
         쿼리의 조건문에 ID, PASSWORD를 넣는다.
         위에 캐싱해 놓은 info에 정보를 전부 담는다.

         1. Connection상태확인 -> Open상태여야한다.
         2. Reader의 상태가 읽고 있는 상황인지 확인해야함
         3. SQL은 Reader를 한개만 사용할 수 있기 때문에 데이터를 모두 읽었다면 Reader의 상태를 Close해줘야한다.
         */
        try
        {
            if(!Connection_Check(connection))
            {
                return false;
            }
            string SQL_Command = string.Format($@"SELECT User_Name, User_Password, User_PhoneNum FROM user_info WHERE User_Name = '{ID}' && User_Password = '{Password}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {//reader가 읽은 데이터가 1개이상 존재합니까?
                //읽은 데이터를 하나씩 나열해야한다.
                while(reader.Read())//reader가 읽고 있는 상황이라면
                {
                    //삼항연산자 - bool ? true : false;
                    string name = reader.IsDBNull(0) ? string.Empty :(string)reader["User_Name"];
                    string pw = reader.IsDBNull(1) ? string.Empty : (string)reader["User_Password"];
                    string phone = reader.IsDBNull(2) ? string.Empty : (string)reader["User_PhoneNum"];
                    if(!name.Equals(string.Empty)||!pw.Equals(string.Empty))
                    {
                        info = new User_info(name, pw, phone);

                        if (!reader.IsClosed)
                            reader.Close();

                        return true;
                    }
                    else
                    {
                        Debug.Log("로그인 실패");
                        break;
                    }
                }//while 종료
            }//if종료
            if (!reader.IsClosed)
                reader.Close();
            return false;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
                reader.Close();
            return false;
        }
    }

    public bool Join(string ID, string Password, string PhoneNum)
    {
        try
        {
            if (!Connection_Check(connection))
            {
                return false;
            }
            string SQL_Command = string.Format(@"INSERT INTO user_info VALUES ('{0}', '{1}', {2});",ID,Password, PhoneNum);
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
                reader.Close();
            return false;
        }
    }

    public bool Change(string ID, string Password, string PhoneNum)
    {
        try
        {
            if (!Connection_Check(connection))
            {
                return false;
            }
            string SQL_Command = string.Format(@"UPDATE user_info SET User_Password = '{1}', User_PhoneNum = '{2}' WHERE User_Name = '{0}';", ID, Password, PhoneNum);
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (!reader.IsClosed)
                reader.Close();
            return false;
        }
    }

    public int Check_Name(string ID)
    {
        // 0 = 연결 실패
        // 1 = 동일한 사용자 존재
        // 2 = 사용할 수 있는 이름입니다.
        try
        {
            if (!Connection_Check(connection))
            {
                return 0;
            }
            string SQL_Command = string.Format($@"SELECT User_Name FROM user_info WHERE User_Name = '{ID}';");
            MySqlCommand cmd = new MySqlCommand(SQL_Command, connection);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {//reader가 읽은 데이터가 1개이상 존재합니까?
                //읽은 데이터를 하나씩 나열해야한다.
                while (reader.Read())//reader가 읽고 있는 상황이라면
                {
                    //삼항연산자 - bool ? true : false;
                    string name = reader.IsDBNull(0) ? string.Empty : (string)reader["User_Name"];
                    if (!name.Equals(string.Empty))
                    {
                        if (!reader.IsClosed)
                            reader.Close();

                        return 1;
                    }
                    return 1;
                }//while 종료
            }//if종료
            if (!reader.IsClosed)
                reader.Close();
            return 2;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            
            return 0;
        }
    }


}
