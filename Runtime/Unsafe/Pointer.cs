namespace Stereome
{
    /// <summary>
    /// Store pointer in struct. Use for unsafe reference.
    /// </summary>
    /// <typeparam name="T">unmanaged type</typeparam>
    public unsafe struct Pointer<T> where T : unmanaged
    {
        public T* Ptr;
        public void SetPointer(ref T component)
        {
            fixed (T* ptr = &component)
            {
                Ptr = ptr;
            }
        }

        public T GetValue()
        {
            return *Ptr;
        }
    }
}