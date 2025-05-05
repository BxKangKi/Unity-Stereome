namespace Stereome
{
    public interface IFixedUpdate : IUpdateSystem
    {
        void OnFixedUpdate(int priority);
    }
}