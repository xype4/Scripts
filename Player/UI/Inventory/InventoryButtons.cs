using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{
    public GameObject Items;
    public GameObject Quests;
    public GameObject Equip;

    public GameObject ItemsButton;
    public GameObject QuestsButton;
    public GameObject EquipButton;

    public void Items_()
    {
        Items.SetActive(true);
        ItemsButton.GetComponent<Image>().color = new Color (104/255f, 104/255f, 104/255f);
        Quests.SetActive(false);
        QuestsButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
        Equip.SetActive(false);
        EquipButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
    }
    public void Quests_()
    {
        Items.SetActive(false);
        ItemsButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
        Quests.SetActive(true);
        QuestsButton.GetComponent<Image>().color = new Color (104/255f, 104/255f, 104/255f);
        Equip.SetActive(false);
        EquipButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
    }
    public void Equip_()
    {
        Items.SetActive(false);
        ItemsButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
        Quests.SetActive(false);
        QuestsButton.GetComponent<Image>().color = new Color (77/255f, 32/255f, 0);
        Equip.SetActive(true);
        EquipButton.GetComponent<Image>().color = new Color (104/255f, 104/255f, 104/255f);
    }

}
