
using System;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName ="Player Stats",menuName ="Somet/TDS/Create Player Stats")]
public class PlayerStats : ActorStats
{
    [Header("Level up base:")]
    public int level;
    public int maxLevel;
    public float xp;
    public float xpToLevelUp;
    [Header("Level up")]
    public float nextXpToLevelUp;
    public float hpUp;
    public override bool IsMaxLevel()
    {
        return level >= maxLevel;
    }
    public override void Load()
    {
        if (!string.IsNullOrEmpty(Prefs.playerData))
        {
            JsonUtility.FromJsonOverwrite(Prefs.playerData, this);
        }
    }
    public override void Save()
    {
        Prefs.playerData=JsonUtility.ToJson(this);
    }
    public override void Upgrade(Action OnSuccess = null, Action OnFailed = null)
    {
       
       while(xp>=xpToLevelUp && !IsMaxLevel())
        {
            level++;
            xp-=xpToLevelUp;
            hp+=hpUp*Helper.GetUpgradeFormula(level);
            xpToLevelUp += nextXpToLevelUp * Helper.GetUpgradeFormula(level);
            Save();
            OnSuccess?.Invoke();
        }
       if(xp<xpToLevelUp||IsMaxLevel()) {
            OnFailed?.Invoke();
        }
    }
}
