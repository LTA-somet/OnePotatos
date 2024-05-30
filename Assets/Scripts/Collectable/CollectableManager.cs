using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CollectableItem
{
    [Range(0f, 1f)]
    public float spawnRate;
    public int amount;
    public Collectable CollectablePrefab;
}
public class CollectableManager : Singleton<CollectableManager>
{
    [SerializeField] private CollectableItem[] m_items;
    public void Spawn(Vector3 position)
    {
        if (m_items == null || m_items.Length <= 0) return;
        float spawnRateChecking=Random.value;
        for (int i = 0; i < m_items.Length; i++)
        {
            var item = m_items[i];
            if (item == null | item.spawnRate < spawnRateChecking) continue;
            CreateCollectable(position, item);  
        }
    }
    private void CreateCollectable(Vector3 spawnPosition,CollectableItem collectableIteam)
    {
        if(collectableIteam == null) return;
        for (int i = 0; i < collectableIteam.amount; i++)
        {
            Instantiate(collectableIteam.CollectablePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
