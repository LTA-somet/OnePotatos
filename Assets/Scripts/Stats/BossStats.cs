using UnityEngine;
[CreateAssetMenu(fileName = "Boss Stats", menuName = "Somet/TDS/ Create Boss Stats")]
public class BossStats : ActorStats
{
    [Header("XP Bonus:")]
    public float minXPBonus;
    public float maxXPBonus;
    [Header("Level Up:")]
    public float hpUp;
    public float damageUp;

}
