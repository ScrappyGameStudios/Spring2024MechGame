using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image _resourceSprite;
    [SerializeField] private TMP_Text _resourceText;
    
    public void UpdateResourceBar(float maxResource, float currentResource)
    {
        _resourceSprite.fillAmount = currentResource / maxResource;
        //_resourceText.text = currentResource.ToString();
    }

    public void ShowResourceBar()
    {
        gameObject.SetActive(true);
    }
    public void HideResourceBar()
    {
        gameObject.SetActive(false);
    }
}
