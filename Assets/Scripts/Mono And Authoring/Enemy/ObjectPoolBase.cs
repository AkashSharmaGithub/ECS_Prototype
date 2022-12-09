using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Entities;
[System.Serializable]
public class ObjectPoolBase : MonoBehaviour
{
   
    protected Enemy prefab;
    protected Transform gameObjectHolder;
   //public ObjectPoolBase(Enemy prefab,Transform PrefabHolder,int maxEnemiesOnScreen)
   // {
       
   //     this.prefab = prefab;
   //     parent = PrefabHolder;
   //     pool = new ObjectPool<Enemy>(createFunc: createPreafab, actionOnGet: actionOnGetPrefab, actionOnRelease: actionOnReleasePrefab, actionOnDestroy: actionOnDestroyPrefab, collectionCheck: false,defaultCapacity:maxEnemiesOnScreen,maxSize:maxEnemiesOnScreen);
   // }
    
    public virtual Enemy createPrefab()
    {
        Enemy spawnedPrefab = Instantiate(prefab);
        //spawn Entity to follow
        return spawnedPrefab;
    }
    public virtual void actionOnGetPrefab(Enemy spawnedObject)
    {
        spawnedObject.gameObject.SetActive(true);
        spawnedObject.transform.parent = gameObjectHolder;
       


    }
    public virtual void actionOnReleasePrefab(Enemy spawnedObject)
    {
        spawnedObject.setPool(null);
        spawnedObject.transform.parent = null;
        spawnedObject.transform.position = Vector3.zero;
        spawnedObject.transform.rotation= Quaternion.identity;
        spawnedObject.gameObject.SetActive(false);
    }
    public virtual void actionOnDestroyPrefab(Enemy spawnedObject)
    {
        Destroy(spawnedObject.gameObject);
    }
  
}
