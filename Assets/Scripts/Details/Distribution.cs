using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[Serializable]
public class Distribution : IDisposable
{
    private static readonly int targetVerticesProperty = Shader.PropertyToID("_TargetVertices");
    private static readonly int targetIndicesProperty = Shader.PropertyToID("_TargetIndices");
    private static readonly int targetTriangleCountProperty = Shader.PropertyToID("_TargetTriangleCount");
    private static readonly int instancesProperty = Shader.PropertyToID("_Instances");

    [SerializeField] private ComputeShader computeShader;

    private ComputeBuffer targetVertices;
    private ComputeBuffer targetIndices;
    private int kernel;

    public void Initialize(Mesh targetMesh)
    {
        GenerateMeshBuffers(targetMesh);
        computeShader.FindKernel("DistributeOnTarget");
    }

    private void GenerateMeshBuffers(Mesh targetMesh)
    {
        var vertices = targetMesh.vertices;
        var normals = targetMesh.normals;
        var indices = targetMesh.GetIndices(0);

        var targetVerticesArray = new Vertex[vertices.Length];
        for (var index = 0; index < vertices.Length; index++)
        {
            targetVerticesArray[index] = new Vertex
            {
                Position = vertices[index],
                Normal = normals[index]
            };
        }

        targetVertices = new ComputeBuffer(vertices.Length, UnsafeUtility.SizeOf<Vertex>(),
            ComputeBufferType.Structured);
        targetVertices.SetData(targetVerticesArray);

        targetIndices = new ComputeBuffer(indices.Length, sizeof(int), ComputeBufferType.Structured);
        targetIndices.SetData(indices);
    }

    public void Distribute(DetailInstanceBuffer buffer)
    {
        var triangleCount = targetIndices.count / 3;
        buffer.Resize(triangleCount);

        computeShader.SetBuffer(kernel, targetVerticesProperty, targetVertices);
        computeShader.SetBuffer(kernel, targetIndicesProperty, targetIndices);
        computeShader.SetFloat(targetTriangleCountProperty, triangleCount);
        computeShader.SetBuffer(kernel, instancesProperty, buffer.Buffer);

        buffer.Buffer.SetCounterValue(0);

        computeShader.Dispatch(kernel, Mathf.CeilToInt(triangleCount / 64f), 1, 1);
    }

    public void Dispose()
    {
        targetVertices?.Dispose();
        targetIndices?.Dispose();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;

        public Vertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}