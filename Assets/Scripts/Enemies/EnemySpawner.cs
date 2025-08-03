using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Enemy> prefabsT1;
        [SerializeField] private List<Enemy> prefabsT2;
        [SerializeField] private List<Enemy> prefabsT3;
        [SerializeField] private float t2Threshold;
        [SerializeField] private float t3Threshold;
        [SerializeField] private float spawnRadius;

        private float _spawnTimer;

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _spawnTimer = SpawnTime(TimeKeeper.Current.GameElapsedTime);
                SpawnWave();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        private void SpawnWave()
        {
            var lst = new List<Enemy>(prefabsT1);

            if (TimeKeeper.Current.GameElapsedTime > t2Threshold)
            {
                lst.AddRange(prefabsT2);
            }

            if (TimeKeeper.Current.GameElapsedTime > t3Threshold)
            {
                lst.AddRange(prefabsT3);
            }
            
            var index = Random.Range(0, lst.Count);
            var prefab = lst[index];
            var enemy = Instantiate(prefab, transform);
            enemy.transform.position = Random.insideUnitCircle * spawnRadius;
        }

        private float SpawnTime(float gameTime)
        {
            return 100 / (gameTime + 100) + 0.15f;
        }
    }
}