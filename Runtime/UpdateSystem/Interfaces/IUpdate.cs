namespace Stereome
{
    public interface IUpdate : IUpdateSystem
    {
        void OnUpdate(int priority);
    }
}