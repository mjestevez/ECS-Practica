using Unity.Entities;

namespace ECS.Components
{
    [GenerateAuthoringComponent]
    public struct Bullet : IComponentData
    {
        public float Speed;
        public float Direction;
        public float CoolDown;
        public float Timer;
    }
}
