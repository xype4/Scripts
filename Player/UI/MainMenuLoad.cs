using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenuLoad : MonoBehaviour
{
    private string loadInfoPath;
    public GameObject LoadingSc;
    void Start()
    {
        loadInfoPath = Application.persistentDataPath + "/LoadInfo.bsa";
    }
     public void LoadGame(int NumSave)   //1-5 сохранения из меню 6-10 быстраые
    {
        LoadingSc.SetActive(true);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(loadInfoPath, FileMode.Create);
        int numsave = NumSave;
        bf.Serialize(fs, numsave);
        fs.Close(); 

        Application.LoadLevel(1);
    }
}
