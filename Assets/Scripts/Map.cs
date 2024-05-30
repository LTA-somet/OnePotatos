
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform playerSpawnPoint;
    [SerializeField] private Transform[] m_aiSpawnPoint;
    public Transform randomAiSpawnPoint
    {
        get
        {
            if (m_aiSpawnPoint == null||m_aiSpawnPoint.Length<=0)return null;
            int randomIdx=Random.Range(0,m_aiSpawnPoint.Length);
            return m_aiSpawnPoint[randomIdx];
        }
    }
       
}
