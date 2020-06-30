using ECS.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Utils;

namespace ECS
{
    public class BulletSpawner : Singleton<BulletSpawner>
    {
        public GameObject blueBulletPrefab;
        public GameObject redBulletPrefab;
        public List<Entity> blueCubes;
        public List<Entity> redCubes;
        public List<Entity> bulletList;
        public float cooldown;
        private Entity blueBulletEntity;
        private Entity redBulletEntity;
        private EntityManager entityManager;
        private BlobAssetStore blobAssetStore;
        public float3 offset;


        private void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            blobAssetStore = new BlobAssetStore();
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
            blueBulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(blueBulletPrefab, settings);
            redBulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBulletPrefab, settings);
            blueCubes = new List<Entity>();
            redCubes = new List<Entity>();
            bulletList = new List<Entity>();
        }


        public void StartSimulation()
        {
            StartCoroutine(BulletsInvoke());
        }
        public void ClearBullets()
        {
            blueCubes.Clear();
            redCubes.Clear();
            for (int i = 0; i < bulletList.Count; i++)
            {
                entityManager.DestroyEntity(bulletList[i]);
            }
            bulletList.Clear();
            StopAllCoroutines();

        }

        private IEnumerator BulletsInvoke()
        {
            yield return new WaitForSeconds(2f);
            while (true)
            {
                if (blueCubes != null)
                {
                    for (int i = 0; i < blueCubes.Count; i++)
                    {
                        float3 pos = entityManager.GetComponentData<Translation>(blueCubes[i]).Value;
                        int direction = entityManager.GetComponentData<MoveForward>(blueCubes[i]).Direction;
                        offset *= direction;
                        pos += offset;

                        Translation t = entityManager.GetComponentData<Translation>(blueBulletEntity);
                        t.Value = pos;
                        entityManager.SetComponentData<Translation>(blueBulletEntity, t);

                        Bullet b = entityManager.GetComponentData<Bullet>(blueBulletEntity);
                        b.Direction = direction;
                        entityManager.SetComponentData<Bullet>(blueBulletEntity, b);

                        Entity bullet = entityManager.Instantiate(blueBulletEntity);
                        bulletList.Add(bullet);

                    }
                }
                if (redCubes != null)
                {
                    for (int i = 0; i < redCubes.Count; i++)
                    {
                        float3 pos = entityManager.GetComponentData<Translation>(redCubes[i]).Value;
                        int direction = entityManager.GetComponentData<MoveForward>(redCubes[i]).Direction;
                        offset *= direction;
                        pos += offset;

                        Translation t = entityManager.GetComponentData<Translation>(redBulletEntity);
                        t.Value = pos;
                        entityManager.SetComponentData<Translation>(redBulletEntity, t);

                        Bullet b = entityManager.GetComponentData<Bullet>(redBulletEntity);
                        b.Direction = direction;
                        entityManager.SetComponentData<Bullet>(redBulletEntity, b);

                        Entity bullet = entityManager.Instantiate(redBulletEntity);
                        bulletList.Add(bullet);
                    }
                }
                yield return new WaitForSeconds(cooldown);
            }

        }

        private void OnDestroy()
        {
            blobAssetStore.Dispose();
        }
    }
}
