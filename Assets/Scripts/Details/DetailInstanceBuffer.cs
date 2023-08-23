using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class DetailInstanceBuffer
{
    private ComputeBuffer buffer;
    private int count;

    public int Count => count;

    public ComputeBuffer Buffer => buffer;

    public DetailInstanceBuffer(int length)
    {
        buffer = new ComputeBuffer(length, UnsafeUtility.SizeOf<DetailInstance>(),
            ComputeBufferType.Append);
    }

    public void Dispose()
    {
        buffer.Dispose();
    }

    public void Resize(int triangleCount)
    {
        if (buffer.count < triangleCount)
        {
            buffer.Dispose();
            buffer = new ComputeBuffer((int)(triangleCount * 1.1f), UnsafeUtility.SizeOf<DetailInstance>(),
                ComputeBufferType.Append);
        }

        count = triangleCount;
    }
}