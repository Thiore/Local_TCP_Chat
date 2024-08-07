using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;

public class User_info
{//Table ������ �ڷ������� ����� ����
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
    //�������� �������� ������ �ݿ���
    public User_info info;
    public MySqlConnection connection; // ����
    public MySqlDataReader reader; // �����͸� ���������� �о���� �ڷ���? Ŭ����?

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
    {//������ �����Ѵٸ� Json�� Ǯ� ����
        if(!File.Exists(path))
        {
            Directory.CreateDirectory(path);//��� ����

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
        //���������� DB���� �����͸� ������ ���� �޼���
        /*
         ���࿡ ��ȸ�Ǵ� �����Ͱ� ���ٸ� ��ȯ�� False
         ��ȸ�� �Ǵ� �����Ͱ� �ִٸ� True�� ������ �ϴµ�
         ������ ���ǹ��� ID, PASSWORD�� �ִ´�.
         ���� ĳ���� ���� info�� ������ ���� ��´�.

         1. Connection����Ȯ�� -> Open���¿����Ѵ�.
         2. Reader�� ���°� �а� �ִ� ��Ȳ���� Ȯ���ؾ���
         3. SQL�� Reader�� �Ѱ��� ����� �� �ֱ� ������ �����͸� ��� �о��ٸ� Reader�� ���¸� Close������Ѵ�.
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
            {//reader�� ���� �����Ͱ� 1���̻� �����մϱ�?
                //���� �����͸� �ϳ��� �����ؾ��Ѵ�.
                while(reader.Read())//reader�� �а� �ִ� ��Ȳ�̶��
                {
                    //���׿����� - bool ? true : false;
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
                        Debug.Log("�α��� ����");
                        break;
                    }
                }//while ����
            }//if����
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
        // 0 = ���� ����
        // 1 = ������ ����� ����
        // 2 = ����� �� �ִ� �̸��Դϴ�.
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
            {//reader�� ���� �����Ͱ� 1���̻� �����մϱ�?
                //���� �����͸� �ϳ��� �����ؾ��Ѵ�.
                while (reader.Read())//reader�� �а� �ִ� ��Ȳ�̶��
                {
                    //���׿����� - bool ? true : false;
                    string name = reader.IsDBNull(0) ? string.Empty : (string)reader["User_Name"];
                    if (!name.Equals(string.Empty))
                    {
                        if (!reader.IsClosed)
                            reader.Close();

                        return 1;
                    }
                    return 1;
                }//while ����
            }//if����
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
