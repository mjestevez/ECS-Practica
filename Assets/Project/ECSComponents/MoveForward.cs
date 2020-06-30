using Unity.Entities;

namespace ECS.Components
{
    [GenerateAuthoringComponent]
    public struct MoveForward : IComponentData
    {
        public float Speed;
        public int Direction;
    }
}
