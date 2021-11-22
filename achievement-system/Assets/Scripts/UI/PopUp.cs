using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public Text Tittle;
    public Text Description;
    public Image Img;
    public Image BackgroundImg;
    public float Durability = 5f;
    public float CurrentTime = 0f;

    private void Awake()
    {
        StartCoroutine(OpenAndClose());
    }

    private void Open()
    {
        this.transform.DOMoveY(500.0f, 0.5f).SetEase(Ease.InBack);
    }

    private void Close()
    {
        this.transform.DOMoveY(800.0f, 0.5f).SetEase(Ease.InBack);
    }

    IEnumerator OpenAndClose()
    {
        yield return new WaitForSeconds(0.1f);
        Open();
        yield return new WaitForSeconds(Durability);
        Close();
        yield return new WaitForSeconds(1f);
        Destroy(this);
    }
}
