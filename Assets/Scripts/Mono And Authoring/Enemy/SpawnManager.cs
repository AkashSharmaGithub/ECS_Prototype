using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int count = 0;
    private float Timer;
    [field:SerializeField]public WeakEnemySpawner weakEnemySpawner { get; private set; }
    

    public bool keepSpawningEnemies;
    [field: SerializeField] public AnimationCurve maxWeakEnemiesInGivenTimeCurve;
    private void Start()
    {
      
        //keepSpawningEnemies = false;
         Task task=spawnEnemies();
      
    }
    private void OnDisable()
    {
        keepSpawningEnemies = false;
       
    }
    private void Update()
    {
        Timer += Time.deltaTime;
    }

    async Task spawnEnemies()
    {
        while(keepSpawningEnemies)
        {
            if(Timer>=600)
            {
                keepSpawningEnemies = false;
            }
            if(count<= maxWeakEnemiesInGivenTimeCurve.Evaluate(Timer))
            {
                Enemy enemy = weakEnemySpawner.pool.Get();
                enemy.setPosition(new Vector3(Random.Range(0, 50f), 2, Random.Range(0, 50f)));
                
                count++;
            }
            await Task.Delay(1);

        }
      
    }

}
