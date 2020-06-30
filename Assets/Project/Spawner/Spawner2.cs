using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Utils;

namespace ECS
{
    public class Spawner2 : Singleton<Spawner2>
    {
        [Serializable]
        public struct LimitRange
        {
            public float Min;
            public float Max;
        }

        #region Variables

        public GameObject cubePrefab;
        private Entity cubeEntity;
        private EntityManager entityManager;
        private BlobAssetStore blobAssetStore;
        private List<Entity> cubes;

        [Header("Bound Limits")]
        public LimitRange BoundX;
        public LimitRange BoundY;
        public LimitRange BoundZ;

        [Header("Others")]
        public float MoveSpeed = 2f;
        public bool red;
        public float TotalUnits { get; private set; }

        #endregion

        public virtual void Awake()
        {
            TotalUnits = 0;
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            blobAssetStore = new BlobAssetStore();
            GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
            cubeEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, settings);

            cubes = new List<Entity>();
        }

        public void Spawn(int spawnUnits)
        {

            for (int i = 0; i < spawnUnits; i++)
            {
                float3 pos = GetRandomPosition();
                Translation t = entityManager.GetComponentData<Translation>(cubeEntity);
                t.Value = pos;
                entityManager.SetComponentData<Translation>(cubeEntity, t);
                Entity newCube = entityManager.Instantiate(cubeEntity);
                cubes.Add(newCube);
            }
            TotalUnits += spawnUnits;
            if (!red)
                BulletSpawner.Instance.blueCubes = cubes;
            else
                BulletSpawner.Instance.redCubes = cubes;
        }

        public void RemoveCube(Entity cube)
        {
            bool remove = cubes.Remove(cube);
            if (remove) TotalUnits--;
        }
        public void ClearCubes()
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                entityManager.DestroyEntity(cubes[i]);
            }
            cubes.Clear();
            TotalUnits = 0;
        }
        private void OnDestroy()
        {
            blobAssetStore.Dispose();
        }



        #region Spawn Functions

        public float3 GetRandomPosition()
        {
            float positionX = UnityEngine.Random.Range(BoundX.Min, BoundX.Max);
            float positionY = UnityEngine.Random.Range(BoundY.Min, BoundY.Max);
            float positionZ = UnityEngine.Random.Range(BoundZ.Min, BoundZ.Max);

            return new float3(positionX, positionY, positionZ);
        }

        #endregion


    }
}
