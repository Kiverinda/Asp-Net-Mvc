using System;
using System.Threading;

namespace Mvc.CustomThreadPool
{
    public class CustomTask : ITask
    {
        public CustomTask(int id)
        {
            Id = id;
        }

        public void Execute()
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        public int Id { get; }
    }
}
