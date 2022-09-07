using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public List<ArrowStats> arrows;

    void Start()
    {
        ArrowStats first = new ArrowStats();                                        //Обычная стрела
        first.id = 2002; first.count = 0; first.flyRange = 0.8f; first.damage = 5; first.speed = 1f;
        arrows.Add(first);

        ArrowStats second = new ArrowStats();                                        //Хорошая стрела
        second.id = 2001; second.count = 0; second.flyRange = 1.3f; second.damage = 7; second.speed = 1.5f;
        arrows.Add(second);
    }
}
