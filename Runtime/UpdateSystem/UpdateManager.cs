using System;
using System.Collections;
using System.Collections.Generic;

namespace Stereome
{
    /// <summary>
    /// UpdateManager for OnUpdate() method instead of using event Update() method.
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Using anonymous class to List<> type
        private readonly List<(IUpdate value, int priority)> updates = new List<(IUpdate, int)>(0);
        private readonly List<(IFixedUpdate value, int priority)> fixedUpdates = new List<(IFixedUpdate, int)>(0);
        private readonly List<(ILateUpdate value, int priority)> lateUpdates = new List<(ILateUpdate, int)>(0);


        /// <summary>
        /// Add IUpdateSystem to UpdateManager. Almost case called in OnEnable()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">(IUpdateSystem) Value to add</param>
        /// <param name="type">Specific type of IUpdateSystem. Value will be regiester as its type.</param>
        /// <param name="priority">Execution prority. Lower is first</param>
        public static void Add<T>(T value, Type type, int priority = 0) where T : IUpdateSystem
        {
            if (Instance != null)
            {
                Instance.AddByType(value, type, priority);
            }
        }

        /// <summary>
        /// Add IUpdateSystem to UpdateManager. Almost case called in OnEnable()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">(IUpdateSystem) Value to add. Registered type will be its type.</param>
        /// <param name="priority">Execution prority. Lower is first</param>
        public static void Add<T>(T value, int priority = 0) where T : IUpdateSystem
        {
            if (Instance != null)
            {
                Instance.AddByValue(value, priority);
            }
        }

        /// <summary>
        /// Add IUpdateSystem to UpdateManager. Almost case called in OnDisable()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static void Remove<T>(T value) where T : IUpdateSystem
        {
            if (Instance != null)
            {
                Instance.Remove_Internal(value);
            }
        }


        // Unity built-in Update event method.
        private void Update()
        {
            for (int i = 0; i < updates.Count; i++)
            {
                updates[i].value.OnUpdate(updates[i].priority);
            }
        }

        // Unity built-in FixedUpdate event method.
        private void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdates.Count; i++)
            {
                fixedUpdates[i].value.OnFixedUpdate(fixedUpdates[i].priority);
            }
        }

        // Unity built-in LateUpdate event method.
        private void LateUpdate()
        {
            for (int i = 0; i < lateUpdates.Count; i++)
            {
                lateUpdates[i].value.OnLateUpdate(lateUpdates[i].priority);
            }
        }


        private void AddByType<T>(T value, Type type, int priority) where T : IUpdateSystem
        {
            if (type == typeof(IUpdate))
            {
                StartCoroutine(AddUpdate(value as IUpdate, priority));
            }
            else if (type == typeof(IFixedUpdate))
            {
                StartCoroutine(AddFixedUpdate(value as IFixedUpdate, priority));
            }
            else if (type == typeof(ILateUpdate))
            {
                StartCoroutine(AddLateUpdate(value as ILateUpdate, priority));
            }
        }


        private void AddByValue<T>(T value, int priority) where T : IUpdateSystem
        {
            if (value is IUpdate)
            {
                StartCoroutine(AddUpdate(value as IUpdate, priority));
            }
            if (value is IFixedUpdate)
            {
                StartCoroutine(AddFixedUpdate(value as IFixedUpdate, priority));
            }
            if (value is ILateUpdate)
            {
                StartCoroutine(AddLateUpdate(value as ILateUpdate, priority));
            }
        }


        private IEnumerator AddUpdate(IUpdate value, int priority)
        {
            yield return null;
            yield return null;
            updates.Add((value, priority));
            updates.Sort((a, b) => a.priority.CompareTo(b.priority));
        }

        private IEnumerator AddFixedUpdate(IFixedUpdate value, int priority)
        {
            yield return null;
            yield return null;
            fixedUpdates.Add((value, priority));
            updates.Sort((a, b) => a.priority.CompareTo(b.priority));
        }

        private IEnumerator AddLateUpdate(ILateUpdate value, int priority)
        {
            yield return null;
            yield return null;
            lateUpdates.Add((value, priority));
            lateUpdates.Sort((a, b) => a.priority.CompareTo(b.priority));
        }


        private void Remove_Internal<T>(T value) where T : IUpdateSystem
        {
            if (value is IUpdate)
            {
                updates.RemoveAll(updates => updates.value == value as IUpdate);
            }
            if (value is IFixedUpdate)
            {
                fixedUpdates.RemoveAll(fixedUpdates => fixedUpdates.value == value as IFixedUpdate);
            }
            if (value is ILateUpdate)
            {
                lateUpdates.RemoveAll(lateUpdates => lateUpdates.value == value as ILateUpdate);
            }

        }

        private void OnDestroy()
        {
            ResetList(updates);
            ResetList(fixedUpdates);
            ResetList(lateUpdates);
        }

        private void ResetList<T>(List<T> list)
        {
            list.Clear();
            list.TrimExcess();
        }
    }
}