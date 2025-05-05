using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Stereome
{
    public static class MonoBehaviorExtensions
    {
        public static Task WaitUntilTask(this MonoBehaviour behaviour, System.Func<bool> predicate)
        {
            var tcs = new TaskCompletionSource<bool>();
            behaviour.StartCoroutine(WaitUntilCoroutine(predicate, tcs));
            return tcs.Task;
        }

        static IEnumerator WaitUntilCoroutine(System.Func<bool> predicate, TaskCompletionSource<bool> tcs)
        {
            yield return new WaitUntil(predicate);
            tcs.SetResult(true); // Task 완료
        }
    }
}
