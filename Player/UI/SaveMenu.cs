using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public SaveLoad SaveLoadMenu;
    public void SaveGame(int NumSave)   //1-5 сохранения из меню
    {
        SaveLoadMenu.SaveButton(NumSave);
    }

    public void LoadGame(int NumSave)   //1-5 сохранения из меню 6-10 быстраые
    {
        SaveLoadMenu.LoadButton(NumSave);
    }
}
