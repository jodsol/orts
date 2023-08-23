using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct DetailInstance
{
    public Matrix4x4 Transform;
    public float Variation;
}