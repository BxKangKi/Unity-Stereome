namespace Stereome
{
    public interface ILateUpdate : IUpdateSystem
    {
        void OnLateUpdate(int priority);
    }
}