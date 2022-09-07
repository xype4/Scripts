using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SettingsSave : MonoBehaviour
{
    public float Sensetive;
    private string savePath;
    public GameObject SensetiveSlider;
    void Start()
    {
        savePath = Application.persistentDataPath + "/Settings.info";
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(savePath, FileMode.Open);
            float sens = (float)bf.Deserialize(fs);
            fs.Close();
            Sensetive = sens;
        }
        else
            Sensetive = 3;
        SensetiveSlider.GetComponent<Slider>().value = Sensetive;
    }

    public void Sens(float sens)
    {
        Sensetive = sens;
    }

    public void SettingSave()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(savePath, FileMode.Create);
        float sens = Sensetive;
        bf.Serialize(fs, sens);
        fs.Close();  
    }
}
