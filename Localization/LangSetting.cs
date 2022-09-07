using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LangSetting : MonoBehaviour
{
    string LangPath;
    int LangToggle_; // 0 - русский 1 - английский
    void Start()
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

        gameObject.GetComponent<Dropdown>().value = LangToggle_;
    }
}
