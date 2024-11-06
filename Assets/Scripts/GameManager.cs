using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;

    public static GameManager instance;
    // Start is called before the first frame update
    private void Awake()
    {

        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

    }
    void Start()
    {
        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MainMenu()
    {
        OnMainMenu?.Invoke();
        Debug.Log("Main Menu Activated");
    }
    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
        Debug.Log(" Items Menu Activated");
    }
    public void ARPosition() 
    {
        OnARPosition?.Invoke();
        Debug.Log("AR Position Activated");
    }
    public void CloseAPP()
    {
        Application.Quit();
    }
}
