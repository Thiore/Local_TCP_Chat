using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Menu;

    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(FindObjectOfType<GameManager>() != this)
            Destroy(FindObjectOfType<GameManager>().gameObject);
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

    public void SurverScene()
    {
        SceneManager.LoadScene("Server");
    }
    public void ClientScene()
    {
        SceneManager.LoadScene("Client");
    }

}
