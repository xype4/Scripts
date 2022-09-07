using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChange : MonoBehaviour
{
    public Item ItemChange;
    public byte CellNumber;
    public bool Equip = false;

    void Start()
    {
        ItemChange = new Item();
    }
}
