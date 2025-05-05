using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using System;

namespace Stereome
{
    /// <summary>
    /// Unsafe utility struct.
    /// Include MemCpy(), LogAddress(), etc..
    /// </summary>
    public struct Unsafe
    {
        /// <summary>
        /// UnsafeUtility.MemCpy for not using '/unsafe' dynamic library.
        /// </summary>
        /// <typeparam name="T">unmanaged type</typeparam>
        /// <param name="destination">Pointer struct type to be copied from source</param>
        /// <param name="source">source to copy</param>
        public static unsafe void MemCpy<T>(Pointer<T> destination, T source) where T : unmanaged
        {
            unsafe
            {
                UnsafeUtility.MemCpy(destination.Ptr, &source, sizeof(T));
            }
        }

        public static unsafe void LogAddress<T>(T source) where T : unmanaged
        {
            TypedReference tr1 = __makeref(source);
            IntPtr ptr1 = **(IntPtr**)(&source);
            Debug.Log(ptr1);
        }
    }
}