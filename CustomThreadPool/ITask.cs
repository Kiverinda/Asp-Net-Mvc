namespace Mvc.CustomThreadPool
{
    public interface ITask
    {
        void Execute();

        int Id { get; }
    }
}
