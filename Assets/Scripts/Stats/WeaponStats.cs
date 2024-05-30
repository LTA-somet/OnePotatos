using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Somet/TDS/Create Weapon Stats")]

public class WeaponStats : Stats
{
    [Header("Base Stats:")]
    public int bullets;
    public float firerate;
    public float reloadTime;
    public float damage;
    [Header("Upgrade:")]
    public int level;
    public int maxLevel;
    public int bulletsUp;
    public float firerateUp;
    public float reloadTimeUp;
    public float damageUp;
    public int upgradePrice;
    public int upgradePriceUp;
    [Header("Limit:")]
    public float minFirerateUp;
    public float minReloadTimeUp;

    public int bulletsUpInfo { get => bulletsUp * (level + 1); }
    public float firerateInfo { get => firerate * Helper.GetUpgradeFormula(level + 1); }
    public float reloadTimeUpInfo { get => reloadTimeUp * Helper.GetUpgradeFormula(level + 1); }
    public float damageUpInfo { get => damageUp * Helper.GetUpgradeFormula(level + 1); }
    public override bool IsMaxLevel()
    {
        return level >= maxLevel;
    }
    public override void Load()
    {
        if (!string.IsNullOrEmpty(Prefs.playerWeaponData))
        {
            JsonUtility.FromJsonOverwrite(Prefs.playerWeaponData, this);
        }
    }
    public override void Save()
    {
        Prefs.playerWeaponData = JsonUtility.ToJson(this);
    }

    public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
    {
      
        if(Prefs.IsEnoughCoins(upgradePrice)&&!IsMaxLevel())
        {
            Prefs.coins -= upgradePrice;
            level++;
            bullets += bulletsUp * level;
            firerate -= firerateUp * Helper.GetUpgradeFormula(level);
            firerate = Mathf.Clamp(firerate, minFirerateUp, firerate);
            reloadTime-= reloadTimeUp*Helper.GetUpgradeFormula(level);
            reloadTime=Mathf.Clamp(reloadTime,minFirerateUp, reloadTime);
            damage += damageUp * Helper.GetUpgradeFormula(level);
            upgradePrice += upgradePriceUp * level;
            Save();
            OnSuccess?.Invoke();
            return;
        }
        OnFailed?.Invoke();
    }
}
