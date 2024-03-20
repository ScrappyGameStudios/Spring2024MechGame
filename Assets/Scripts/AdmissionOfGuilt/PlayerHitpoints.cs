using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerArsenal))]
public class PlayerHitpoints : MonoBehaviour, IDamageable
{
    private PlayerArsenal pa;
    private bool hasShields;

    [Header("Resource Bars")]
    [SerializeField]
    private ResourceBar ArmorBar;
    [SerializeField]
    private ResourceBar ShieldBar;

    #region Hitpoints
    [Header("Armor")]
    [SerializeField]
    private float maxArmor = 800.0f;
    private float currentArmor;

    [Header("Shield Generator")]
    [SerializeField]
    private float maxShields = 400.0f;
    [SerializeField]
    private float shieldRechargeRate = 50.0f;
    [SerializeField]
    private float shieldRechargeWait = 8.0f;
    [SerializeField]
    private float shieldDepletePenalty = 20.0f;
    private float currentShields;
    private float shieldRechargeTime;
    #endregion Hitpoints

    #region IDamageable
    public void Damage(float damage)
    {
        // check shields if they exist
        if (hasShields && currentShields > 0f)
        {
            // account for excess damage
            if (damage > currentShields)
            {
                damage -= currentShields;
                currentShields = 0f;
                shieldRechargeTime = shieldDepletePenalty;
            }
            else
            {
                currentShields -= damage;
                damage = 0f;
                shieldRechargeTime = shieldRechargeWait;
            }

            ShieldBar.UpdateResourceBar(maxShields, currentShields);
        }
        
        // check if armor exists
        if (damage > 0f && currentArmor > 0f)
        {
            shieldRechargeTime = shieldDepletePenalty;

            if (damage > currentArmor)
            {
                currentArmor = 0f;
                ArmorBar.UpdateResourceBar(maxArmor, currentArmor);
                Kill();
            }
            else
            {
                currentArmor -= damage;
                ArmorBar.UpdateResourceBar(maxArmor, currentArmor);
            }
        }
    }

    public void Repair(float healing)
    {
        
    }

    public void Generate(float charge)
    {

    }

    public void Kill()
    {
        
    }
    #endregion IDamageable

    // Start is called before the first frame update
    void Start()
    {
        // initialize Armor Hitpoints
        currentArmor = maxArmor;
        ArmorBar.UpdateResourceBar(maxArmor, currentArmor);
        ArmorBar.ShowResourceBar();

        // initialize Shield Hitpoints
        if (pa.Back == BackEquipment.ShieldGenerator)
        {
            hasShields = true;
            currentShields = maxShields;
            ShieldBar.UpdateResourceBar(maxShields,currentShields);
            ShieldBar.ShowResourceBar();
        }
        else 
        {
            hasShields = false;
            ShieldBar.HideResourceBar(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasShields && currentShields < maxShields) ShieldRecharge();
    }

    private void ShieldRecharge()
    {
        // recharge shields
        if (shieldRechargeTime < 0f)
        {
            currentShields = Mathf.Clamp(currentShields + Time.deltaTime * shieldRechargeRate,0f,maxShields);
            ShieldBar.UpdateResourceBar(maxShields, currentShields);
        }
        else
        {
            shieldRechargeTime -= Time.deltaTime;
        }
    }
}

public interface IDamageable
{
    public void Damage(float damage);
    public void Repair(float healing);
    public void Generate(float charge);
    public void Kill();
}

public interface IStatus
{

}