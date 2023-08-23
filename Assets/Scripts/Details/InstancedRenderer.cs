using System;
using UnityEngine;

[Serializable]
public class InstancedRenderer
{
    private static readonly int instances = Shader.PropertyToID("_Instances");

    [SerializeField] private Material material;
    [SerializeField] private Mesh mesh;
    
    public void Render(DetailInstanceBuffer buffer, in Bounds worldBounds)
    {
        material.SetBuffer(instances, buffer.Buffer);
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, worldBounds, buffer.Count);
    }
}