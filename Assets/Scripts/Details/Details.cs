using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Details : MonoBehaviour
{
    [SerializeField] private Distribution distribution;
    [SerializeField] private InstancedRenderer renderer;

    private DetailInstanceBuffer buffer;

    private void Awake()
    {
        buffer = new DetailInstanceBuffer(100);
        distribution.Initialize(GetComponent<MeshFilter>().sharedMesh);
        distribution.Distribute(buffer);
    }

    private void OnDestroy()
    {
        buffer.Dispose();
    }

    private void Update()
    {
        renderer.Render(buffer, new Bounds(transform.position, Vector3.zero));
    }
}