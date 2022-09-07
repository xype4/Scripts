using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SaveSlot : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public int Number;
    public bool Quick;
    public bool Menu;
    private string filePath;
    public GameObject SaveImagePanel;
    public Texture2D SaveImage;

    void Start()
    {
        SaveImagePanel.SetActive(false);

        if (Quick)
            filePath = Application.persistentDataPath + "/Saves/PicQuickSave"+Number+".png";
        if (Menu)
            filePath = Application.persistentDataPath + "/Saves/PicPrivateSave"+Number+".png";
        
        if(File.Exists(filePath))
            StartCoroutine(Image_(filePath));
    }

    public void Restart()
    {
        if(File.Exists(filePath))
            StartCoroutine(Image_(filePath));
    }

    IEnumerator Image_(string url)
    {
        ColorButton();
        var www = new WWW(url);
        yield return www;
        SaveImage = www.texture;
    }
    public void ColorButton()
    {
        if(File.Exists(filePath))
            gameObject.GetComponent<Image>().color = new Color(0/255f, 0/255f, 0/255f);
        else   
            gameObject.GetComponent<Image>().color = new Color(142/255f, 142/255f, 142/255f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(File.Exists(filePath))
        {
            SaveImagePanel.SetActive(true);
            SaveImagePanel.GetComponent<Image>().sprite = Sprite.Create(SaveImage, new Rect(0, 0, SaveImage.width, SaveImage.height), new Vector2(.5f, .5f));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SaveImagePanel.SetActive(false);
    }
}
