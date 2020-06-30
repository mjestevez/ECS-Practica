using ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ECS.System
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class CollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld stepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;
        private int index;

        protected override void OnCreate()
        {
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            index = 0;
            base.OnCreate();
        }
        private struct ApplicationJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<BlueCube> blueCubeGroup;
            [ReadOnly] public ComponentDataFromEntity<RedCube> redCubeGroup;
            [ReadOnly] public ComponentDataFromEntity<BlueBullet> blueBulletGroup;
            [ReadOnly] public ComponentDataFromEntity<RedBullet> redBulletGroup;
            public EntityCommandBuffer.Concurrent commandBuffer;
            public int entityInQueryIndex;
            public void Execute(TriggerEvent triggerEvent)
            {
                if (blueCubeGroup.HasComponent(triggerEvent.Entities.EntityA) && redBulletGroup.HasComponent(triggerEvent.Entities.EntityB))
                {
                    if (triggerEvent.Entities.EntityA != null)
                    {
                        GameManager.Instance.blueSpawn.RemoveCube(triggerEvent.Entities.EntityA);
                        commandBuffer.DestroyEntity(entityInQueryIndex, triggerEvent.Entities.EntityA);
                        commandBuffer.DestroyEntity(entityInQueryIndex + 1, triggerEvent.Entities.EntityB);
                    }

                }
                else
                if (blueCubeGroup.HasComponent(triggerEvent.Entities.EntityB) && redBulletGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    if (triggerEvent.Entities.EntityB != null)
                    {
                        GameManager.Instance.blueSpawn.RemoveCube(triggerEvent.Entities.EntityB);
                        commandBuffer.DestroyEntity(entityInQueryIndex, triggerEvent.Entities.EntityB);
                        commandBuffer.DestroyEntity(entityInQueryIndex + 1, triggerEvent.Entities.EntityA);
                    }
                }
                else
                if (redCubeGroup.HasComponent(triggerEvent.Entities.EntityA) && blueBulletGroup.HasComponent(triggerEvent.Entities.EntityB))
                {
                    if (triggerEvent.Entities.EntityA != null)
                    {
                        GameManager.Instance.redSpawn.RemoveCube(triggerEvent.Entities.EntityA);
                        commandBuffer.DestroyEntity(entityInQueryIndex, triggerEvent.Entities.EntityA);
                        commandBuffer.DestroyEntity(entityInQueryIndex + 1, triggerEvent.Entities.EntityB);
                    }
                }
                else
                if (redCubeGroup.HasComponent(triggerEvent.Entities.EntityB) && blueBulletGroup.HasComponent(triggerEvent.Entities.EntityA))
                {
                    if (triggerEvent.Entities.EntityB != null)
                    {
                        GameManager.Instance.redSpawn.RemoveCube(triggerEvent.Entities.EntityB);
                        commandBuffer.DestroyEntity(entityInQueryIndex, triggerEvent.Entities.EntityB);
                        commandBuffer.DestroyEntity(entityInQueryIndex + 1, triggerEvent.Entities.EntityA);
                    }
                }
            }

        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            index++;
            JobHandle applicationJob = new ApplicationJob
            {
                blueCubeGroup = GetComponentDataFromEntity<BlueCube>(true),
                redCubeGroup = GetComponentDataFromEntity<RedCube>(true),
                blueBulletGroup = GetComponentDataFromEntity<BlueBullet>(true),
                redBulletGroup = GetComponentDataFromEntity<RedBullet>(true),
                commandBuffer = endSimCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                entityInQueryIndex = index

            }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
            endSimCommandBufferSystem.AddJobHandleForProducer(applicationJob);
            return applicationJob;
        }
    }
}
