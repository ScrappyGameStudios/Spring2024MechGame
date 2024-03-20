using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Resource Bars")]
    [SerializeField]
    private GameObject ArmorBar;
    [SerializeField]
    private GameObject ShieldBar;
    [SerializeField]
    private GameObject FuelBar;
    [SerializeField]
    private GameObject ExtendedFuelBar;

    [Header("Combat Interface")]
    [SerializeField]
    private GameObject NormalCrosshair;
    [SerializeField]
    private GameObject AimedCrosshair;
    [SerializeField]
    private GameObject WeaponDisplay;

    [Header("Miscellaneous Interface")]
    [SerializeField]
    private GameObject Compass;
    [SerializeField]
    private GameObject Objective;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
