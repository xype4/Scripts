using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderShop : MonoBehaviour
{
    public Text text;

    public void SetText()
    {
        text.text = gameObject.GetComponent<Slider>().value.ToString();
    }
}
