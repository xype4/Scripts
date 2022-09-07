using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropMob : MonoBehaviour
{
    public GameObject F;

    public Item[] Drop = new Item[4];
    [Range(0,10)]
    public int[] Range = new int[4];

    public Item[] RareDrop = new Item[3];
    [Range(0,10)]
    public int[] RareRange = new int[3];

    public Inventory inventory;
    public GameObject DropPanel;
    int d = 0;
    private int[] c = new int[7];
    private Transform[] DP = new Transform[7];

    void Start()
    {
        for(int i = 6, j = 0; j < 7; i--, j++)
        {
            DP[j] = DropPanel.transform.GetChild(i);
        }

        reload();
    }

    public void reload()
    {
        for (int i = 0; i < 4; i ++)
        {
            c[i] = Random.Range(0,Range[i])/2;
        }

        for (int i = 0; i < 3; i ++)
        {
            c[4+i] = Random.Range(0,RareRange[i])/4;
        }
    }


    void OnTriggerStay(Collider other)           
    {
        if((other.tag == "PlayerHandCol") && Input.GetKey("f"))
        {
            for(int i = 0; i < 4; i++)
            {
                if(Drop[i] != null)
                    inventory.AddItem(Drop[i].id, Drop[i].countItem, Drop[i].inventoryList);
            }
            for(int i = 0; i < 3; i++)
            {
                if(RareDrop[i] != null)
                    inventory.AddItem(RareDrop[i].id, RareDrop[i].countItem, RareDrop[i].inventoryList);
            }

            gameObject.GetComponent<BoxCollider>().enabled = false;
            Exit_F();
            Destroy(gameObject,0);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerHandCol")
        {
            F.SetActive(true);
            d = 0;
            for(int i = 0;  i<7; i++)
            {
                if (c[i] != 0)
                {
                    if(i < 5)
                        DP[d].GetChild(1).GetComponent<Text>().text = Drop[i].Name;
                    else
                        DP[d].GetChild(1).GetComponent<Text>().text = RareDrop[i-4].Name;

                    DP[d].GetChild(0).GetComponent<Text>().text = "x " + c[i].ToString(); 
                    DP[d].gameObject.SetActive(true);
                    d++;
                }
            }
            
            for(int i = 0; i < 4; i++)
            {
                if(Drop[i] != null)
                    Drop[i].countItem = (byte)c[i];
            }
            for(int i = 0; i < 3; i++)
            {
                if(RareDrop[i] != null)
                    RareDrop[i].countItem = (byte)c[i+4];
            }

            DropPanel.SetActive(true);
            for (int  i = d; i < 7; i++)
            {
                DP[i].gameObject.SetActive(false);
            }
            d = 0;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHandCol") 
        {
            F.SetActive(false);
            for (int  i = 0; i < 7; i++)
            {
                DP[i].gameObject.SetActive(false);
                DropPanel.SetActive(false);
            }
        }
    }
    void Exit_F()
    {
        F.SetActive(false);
        for (int  i = 0; i < 7; i++)
        {
            DP[i].gameObject.SetActive(false);
            DropPanel.SetActive(false);
        }
    }

}
