using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // pool list
    private Dictionary<string, List<GameObject>> pool;
    // Start is called before the first frame update
    void Start()
    {
        // init pool
        pool = new Dictionary<string, List<GameObject>>();
        spawnedTerrain = new List<GameObject>();
        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;
        foreach (TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }
    // pool function
    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        if (pool.ContainsKey(item.name))
        {
            // if item available in pool
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else
        {
            // if item list not defined, create new one
            pool.Add(item.name, new List<GameObject>());
        }
        // create new one if no item available in pool
        GameObject newItem = Instantiate(item, parent);
        return newItem;
    }
    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name))
        {
            Debug.LogError("INVALID POOL ITEM!!");
        }
        pool[item.name].Add(item);
        item.SetActive(false);
        private void GenerateTerrain(float posX, TerrainTemplateController forceterrain =
null)
        {
            GameObject item = null;
            if (forceterrain == null)
            {
                item = terrainTemplates[Random.Range(0,

                terrainTemplates.Count)].gameObject;
            }
            else
            {
                item = forceterrain.gameObject;
            }
            //GameObject newTerrain = Instantiate(item, transform);
            GameObject newTerrain = GenerateFromPool(item, transform);
            newTerrain.transform.position = new Vector2(posX, 0f);
            spawnedTerrain.Add(newTerrain);
        }
        private void RemoveTerrain(float posX)
        {
            GameObject terrainToRemove = null;
            // find terrain at posX
            foreach (GameObject item in spawnedTerrain)
            {
                if (item.transform.position.x == posX)
                {
                    terrainToRemove = item;
                    break;
                }
            }
            // after found;
            if (terrainToRemove != null)
            {
                spawnedTerrain.Remove(terrainToRemove);
                //Destroy(terrainToRemove);
                ReturnToPool(terrainToRemove);
            }
        }
    }
}
