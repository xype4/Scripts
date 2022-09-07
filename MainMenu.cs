using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject Main_Menu;
    public GameObject Settings_Menu;
    public GameObject Load_Menu;
    public GameObject LoadingSc;
    private string loadInfoPath;


    void Start()
    {
        loadInfoPath = Application.persistentDataPath + "/LoadInfo.bsa";

        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
    }

    public void NewGame()
    {
        LoadingSc.SetActive(true);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(loadInfoPath, FileMode.Create);
        int numsave = 0;
        bf.Serialize(fs, numsave);
        fs.Close(); 

        Application.LoadLevel(1);
        Cursor.visible = false;
    }

    public void Continue()
    {
        LoadingSc.SetActive(true);

        Application.LoadLevel(1);
        Cursor.visible = false;
    }

    public void Load()
    {
        Main_Menu.SetActive(false);
        Load_Menu.SetActive(true);
    }

    public void Settings()
    {
        Settings_Menu.SetActive(true);
        Main_Menu.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void InMainMenuFromOptions()
    {
        Main_Menu.SetActive(true);
        Settings_Menu.SetActive(false);
    }

    public void InMainMenuFromLoad()
    {
        Main_Menu.SetActive(true);
        Load_Menu.SetActive(false);
    }
}
