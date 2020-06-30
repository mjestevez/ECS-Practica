using ECS.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.System
{
    public class MoveForwardSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            float minX = GameManager.Instance.blueSpawn.BoundX.Min;
            float maxX = GameManager.Instance.redSpawn.BoundX.Max;

            JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref MoveForward moveForward) =>
            {
                translation.Value += new float3(moveForward.Speed * moveForward.Direction * deltaTime, 0f, 0f);
                if (translation.Value.x > maxX)
                {
                    moveForward.Direction *= -1;
                    translation.Value.x = maxX;
                }
                if(translation.Value.x < minX)
                {
                    moveForward.Direction *= -1;
                    translation.Value.x = minX;
                }
            }).Schedule(inputDeps);
            return jobHandle;
        }
    }
}
