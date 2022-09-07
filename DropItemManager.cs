using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    public GameObject dataBase;
    public Item[] DropItem;
    public List<Item> PlayerItem = new List<Item>();

    public void LoadData(List<Save.Item_Drop> savePlayer, List<Save.Item_Drop> saveDrop)
    {
        int max = dataBase.transform.childCount;
        for(int i = 0; i < max; i++)
        {
            if(saveDrop[i].ID != 0)
            {
                dataBase.transform.GetChild(i).position = new Vector3(saveDrop[i].Position.x, saveDrop[i].Position.y, saveDrop[i].Position.z);

            }
            else
            {
               Destroy (dataBase.transform.GetChild(i).gameObject);
            }
        }
        max = savePlayer.Count;
        int max2 = dataBase.transform.parent.GetChild(0).childCount;
        for(int i = 0; i < max; i++)
        {
            for(int j = 0; j < max2; j++)
            if(savePlayer[i].ID == dataBase.transform.parent.GetChild(0).GetChild(j).GetComponent<Item>().id)
                {
                    Item item = dataBase.transform.parent.GetChild(0).GetChild(j).GetComponent<Item>();
                    item.countItem = (byte)savePlayer[i].Count;

                    GameObject droppedItem = Instantiate(item.gameObject);
                    droppedItem.transform.position = new Vector3(savePlayer[i].Position.x, savePlayer[i].Position.y+2, savePlayer[i].Position.z);
                    droppedItem.transform.GetComponent<Rigidbody>().isKinematic = false;
                    droppedItem.transform.SetParent(dataBase.transform.parent.GetChild(2));
                    break;
                }          
        }
    }
}
