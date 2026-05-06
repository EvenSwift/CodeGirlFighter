using System;
using System.Collections.Generic;
using Main.Scripts.Framework.Controller.Base.Interface;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main.Scripts.Framework.Controller.Base
{
    public class ResourceTask : IMultipleTask
    {
        public int TaskCount => _taskList.Count + _subTaskCount;
        public float Progress { get; private set; }
        public event Action<ITask> Perform;
        public event Action<ITask> Completed;
        private readonly List<Func<AsyncOperationHandle>> _taskList = new();
        private int _subTaskCount;
        private int _loadedCount;

        public void RegisterTask(Func<AsyncOperationHandle> task)
        {
            _taskList.Add(task);
        }

        public void RegisterSubTask(int count)
        {
            _subTaskCount += count;
        }

        public void Execute()
        {
            _loadedCount = 0;
            Progress = 0f;

            foreach (var task in _taskList)
            {
                var handle = task.Invoke();
                if (handle.IsDone)
                {
                    OnLoaded();
                }
                else
                {
                    handle.Completed += _ => { OnLoaded(); };
                }
            }
        }

        public void OnSubTaskCompleted()
        {
            OnLoaded();
        }

        private void OnLoaded()
        {
            _loadedCount++;
            var taskCount = _taskList.Count + _subTaskCount;
            Progress = (float)_loadedCount / taskCount;
            Perform?.Invoke(this);
            if (_loadedCount != taskCount)
            {
                return;
            }

            Completed?.Invoke(this);
            _taskList.Clear();
            _subTaskCount = 0;
        }
    }
}
