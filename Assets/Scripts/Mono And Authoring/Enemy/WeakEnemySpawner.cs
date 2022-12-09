using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Entities;
public class WeakEnemySpawner : ObjectPoolBase
{

    public GameObject weakEnemyEntityGameObject;
    public Entity weakEnemyEntity;
    
    [field:SerializeField]public Enemy enemyPrefab { get; private set; }
    [field: SerializeField] public Transform spawnedPrefabHolder { get; private set; }
    [field: SerializeField] public int maximumSpawnableEnemies { get; private set; }
    [field: SerializeField] public Transform objectToAttack { get; private set; }
   // [field: SerializeField] public PrefabsCollectionComponent prefabsCollectionComponent { get; private set; }
    public ObjectPool<Enemy> pool { get; private set; }


    private BlobAssetStore blobAssetStore;
    private EntityManager entityManager;
    private void setVariablesOfBaseClass()
    {
        base.prefab = enemyPrefab;
        base.gameObjectHolder = spawnedPrefabHolder;

    }
    private void Awake()
    {
        setVariablesOfBaseClass();
        pool = new ObjectPool<Enemy>(createFunc: createPrefab, actionOnGet: actionOnGetPrefab, actionOnRelease: actionOnReleasePrefab, actionOnDestroy: actionOnDestroyPrefab, collectionCheck: false, defaultCapacity: maximumSpawnableEnemies, maxSize: maximumSpawnableEnemies);
        
        
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
         blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        weakEnemyEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(weakEnemyEntityGameObject, settings);
        Debug.Log("entity:"+weakEnemyEntity);
        Destroy(weakEnemyEntityGameObject);
       
    }
    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
    public override void actionOnGetPrefab(Enemy spawnedObject)
    {
        base.actionOnGetPrefab(spawnedObject);
        spawnedObject.setObjectToAttack(objectToAttack);
        spawnedObject.setPool(pool);
        Entity entity=entityManager.Instantiate(weakEnemyEntity);
       if( spawnedObject.TryGetComponent<EntityFollowGameObject>(out EntityFollowGameObject followScript))
            {
            //followScript.followGameObjectComponent = entityManager.GetComponentData<FollowGameObjectComponent>(entity);
            followScript.setEntityToFollowGameObject(entity);
            
            if(spawnedObject.TryGetComponent<ThrowEnemyAwayFromPlayer>(out ThrowEnemyAwayFromPlayer throwEnemyAway))
                {
                EnemyComponent enemyComponent=entityManager.GetComponentData<EnemyComponent>(entity);
               
                entityManager.SetComponentData<EnemyComponent>(entity, enemyComponent);
            }

            if(spawnedObject.TryGetComponent<ThrowEnemyAwayFromPlayer>(out ThrowEnemyAwayFromPlayer throwEnemyAwayFromPlayer))
            {
                throwEnemyAwayFromPlayer.EnemyEntity = entity;
            }
        }
        //entityManager.GetComponentData<FollowGameObjectComponent>(entity).gameObjectToFollow = this.transform;
     
    }

 
}
