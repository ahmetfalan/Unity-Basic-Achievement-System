using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance;

    public PopUp popUp;
    public GameObject canvas;

 
    private void Awake()
    {
        Instance = this;
    }

    public void Create(string tittle, string description, float durability)
    {
        GameObject panel = Instantiate(popUp.gameObject, new Vector2(transform.position.x, 800), Quaternion.identity);
        panel.transform.SetParent(canvas.transform);
        panel.transform.GetComponent<PopUp>().Tittle.text = tittle;
        panel.transform.GetComponent<PopUp>().Description.text = description;
        panel.transform.GetComponent<PopUp>().Durability = durability;
    }

}

