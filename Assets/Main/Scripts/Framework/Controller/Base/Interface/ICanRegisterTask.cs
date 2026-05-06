using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main.Scripts.Framework.Controller.Base.Interface
{
    public interface ITask
    {
        float Progress { get; }
        event Action<ITask> Perform;
        event Action<ITask> Completed;
        void Execute();
    }

    public interface IMultipleTask : ITask, ICanRegisterTask
    {
        int TaskCount { get; }
    }

    public interface ICanRegisterTask
    {
        void RegisterTask(Func<AsyncOperationHandle> task);

        /// <summary>
        /// 注册子任务，只计算数量，子任务完成时需要调用 OnSubTaskCompleted();
        /// </summary>
        /// <param name="count"></param>
        void RegisterSubTask(int count);

        void OnSubTaskCompleted();
    }
}
