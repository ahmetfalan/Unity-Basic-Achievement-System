using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance;

    [SerializeField] private Text tittle;
    [SerializeField] private Text description;
    [SerializeField] private Image img;
    [SerializeField] private Image backimg;
    [SerializeField] private float timeRemaining = 5f;

    private void Start()
    {
        Instance = this;
    }

    public void Open(string tittle, string description, float durability)
    {
        this.tittle.text = tittle;
        this.description.text = description;
        this.timeRemaining = durability;
        //this.img = img;
        //this.backimg = backimg;
        this.gameObject.transform.DOMoveY(500.0f, 0.5f).SetEase(Ease.InBack);
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            this.gameObject.transform.DOMoveY(700.0f, 0.1f).SetEase(Ease.InBack);
            timeRemaining = 5f;
        }
    }
}
