using ECS.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

namespace ECS.System
{
    public class BulletSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem endSimCommandBufferSystem;


        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            EntityCommandBuffer.Concurrent commandBuffer = endSimCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
            JobHandle jobHandle = Entities.ForEach((int entityInQueryIndex, Entity entity, ref Translation translation, ref Bullet bullet) =>
            {
                translation.Value += new float3(bullet.Speed * bullet.Direction * deltaTime, 0f, 0f);
                bullet.Timer += deltaTime;
                if (bullet.Timer >= bullet.CoolDown)
                {
                    commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                }

            }).Schedule(inputDeps);
            endSimCommandBufferSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }

        protected override void OnCreate()
        {
            endSimCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            base.OnCreate();
        }
    }
}
