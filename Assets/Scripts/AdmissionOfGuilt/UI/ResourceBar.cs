using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image _resourceSprite;
    [SerializeField] private GameObject _resourceBar;
    
    public void UpdateResourceBar(float maxResource, float currentResource)
    {
        _resourceSprite.fillAmount = currentResource / maxResource;
    }

    public void ShowResourceBar()
    {
        _resourceBar.SetActive(true);
    }
    public void HideResourceBar()
    {
        _resourceBar.SetActive(false);
    }
}
