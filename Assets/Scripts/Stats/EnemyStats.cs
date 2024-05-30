
using UnityEngine;
[CreateAssetMenu( fileName ="Enenmy Stats",menuName ="Somet/TDS/ Create Enemy Stats")]
public class EnemyStats : ActorStats
{
    [Header("XP Bonus:")]
    public float minXPBonus;
    public float maxXPBonus;
    [Header("Level Up:")]
    public float hpUp;
    public float damageUp;
   
}
