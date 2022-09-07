using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LangToggle : MonoBehaviour
{
    
    public int LangToggle_ = 1; // 0 - русский 1 - английский
    string LangPath;
    public GameObject LangSetting;
    void Awake()
    {
        LangPath = Application.persistentDataPath + "/Lang.pass";
        if (File.Exists(LangPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(LangPath, FileMode.Open);
            int lang = (int)bf.Deserialize(fs);
            fs.Close();
            LangToggle_ = lang;
        }
        else
            LangToggle_ = 1;
    }
    public void Change(int lang)
    {
        LangToggle_ = lang;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(LangPath, FileMode.Create);
        int lang_ = LangToggle_;
        bf.Serialize(fs, lang_);
        fs.Close(); 
    }
}
