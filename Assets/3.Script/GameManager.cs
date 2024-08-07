using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] private GameObject Menu;
    

    public bool isLogin;

   

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isLogin = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Update()
    {
       
        if (!Menu.activeSelf&&Input.GetKeyDown(KeyCode.Escape)&&!SceneManager.GetActiveScene().name.Equals("SelectScene"))
        {
            Menu.SetActive(true);
        }
        else if (Menu.activeSelf&&Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(false);
        }

    }

    public void Continue()
    {
        Menu.SetActive(false);
    }

    public void ReturnSelect()
    {
        Menu.SetActive(false);
        SceneManager.LoadScene("SelectScene");
        
    }

   
}
