using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject Player;
    [HideInInspector]
    public bool inMenu = false;
    public GameObject UI;
    public GameObject Dialoge;
    public GameObject Game_Menu;
    public GameObject Settings_Menu;
    public GameObject Save_Menu;
    public GameObject Load_Menu;
    public GameObject Death;
    public GameObject F;

    public GameObject InventoryUI;
    public Inventory InventoryPlayer;
    public ContaiterVisiable ContVis;
    public ShopVisiable ShopVis;
    public GameObject AlchemyUI;

    public int Menu = 0; // 1- Главное 2- Настройка 3- сохранения 4- загрузка сохранения // 5 -weapons
    void Start()
    {
        StartCoroutine(F_Button());
        Cursor.visible = false;
    }

    void Update()
    {
        if (Player.GetComponent<PlayerStats>(). HP_Count <= 0)
        {
            Death.SetActive(true);
            Cursor.visible = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(InventoryUI.activeSelf == true)
            {
                ContVis.ContainerHide();
                ShopVis.ShopHide();
                AlchemyUI.SetActive(false);
                InventoryPlayer.InventoryHide();
                Cursor.visible = false;
                return;
            }
            if(inMenu)
            {
                switch (Menu) 
                {
                    case 1:
                        Cursor.visible = false;
                        Game_Menu.SetActive(false);
                        inMenu = false;
                        UI.SetActive(true);
                        //Dialoge.SetActive(true);
                        Menu = 0;
                        Time.timeScale = 1;
                        break;
                    case 2:
                        Game_Menu.SetActive(true);
                        Settings_Menu.SetActive(false);
                        Menu = 1;
                        break;
                    case 3:
                        Game_Menu.SetActive(true);
                        Save_Menu.SetActive(false);
                        Menu = 1;
                        break;
                    case 4:
                        Game_Menu.SetActive(true);
                        Load_Menu.SetActive(false);
                        Menu = 1;
                        break;
                }
            }
            else
            {
                Time.timeScale = 0;
                UI.SetActive(false);
                //Dialoge.SetActive(false);
                Cursor.visible = true;
                Game_Menu.SetActive(true);
                inMenu = true;
                Menu = 1;
            }
        }
    }
    public void InGame()
    {
        Cursor.visible = false;
        Game_Menu.SetActive(false);
        inMenu = false;
        UI.SetActive(true);
        //Dialoge.SetActive(true);
        Menu = 0;
        Time.timeScale = 1;
    }
    public void Settings()
    {
        Game_Menu.SetActive(false);
        Settings_Menu.SetActive(true);
        Menu = 2;
    }
    public void Saves()
    {
        Game_Menu.SetActive(false);
        Save_Menu.SetActive(true);
        Menu = 3;
    }
    public void Load()
    {
        Game_Menu.SetActive(false);
        Load_Menu.SetActive(true);
        for(int i = 0; i < Load_Menu.transform.childCount; i++)                                            // Обновление цвета кнопок в меню
        {
            SaveSlot sl = Load_Menu.transform.GetChild(i).gameObject.GetComponent<SaveSlot>();
            if(sl != null)
                Load_Menu.transform.GetChild(i).gameObject.GetComponent<SaveSlot>().ColorButton();
        }
        Menu = 4;
    }
    public void InMenu()
    {
        Application.LoadLevel(0);
    }
    public void InGameMenuFromSettings()
    {
        Game_Menu.SetActive(true);
        Settings_Menu.SetActive(false);
        Menu = 1;
    }
    public void InGameMenuFromSave()
    {
        Game_Menu.SetActive(true);
        Save_Menu.SetActive(false);
        Menu = 1;
    }
    public void InGameMenuFromLoad()
    {
        Game_Menu.SetActive(true);
        Load_Menu.SetActive(false);
        Menu = 1;
    }
    public void Respawn()
    {
        // Application.LoadLevel(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
    IEnumerator F_Button()
    {
        yield return new WaitForSeconds(0.2f);
        F.SetActive(false);      
    }
}
