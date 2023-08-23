using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{

    List<Vector3> mapDatas;
    public float xStart, yStart, xEnd, yEnd,step;
    public float loadTolerance;
    List<Vector3> loadedMapDatas;
    Dictionary<Vector3, MapDataStreamer> mapDataStreamers = new Dictionary<Vector3, MapDataStreamer>();

    private void Start()
    {

        mapDatas = new List<Vector3>();
        loadedMapDatas = new List<Vector3>();

        for (float x = xStart; x <= xEnd; x += step)
        {

            for (float y = yStart; y <= yEnd; y += step)
            {

                mapDatas.Add(new Vector3(x, 0, y));

            }

        }

    }

    private void Update()
    {

        foreach (Vector3 a in mapDatas)
        {

            if (IsVector3InArea(transform.position, a, loadTolerance) && !loadedMapDatas.Contains(a))
            {

                loadedMapDatas.Add(a);
                mapDataStreamers.Add(a, new MapDataStreamer("Assets/Map Data/MapX" + a.x + "Y" + a.z + ".asset"));

            }
            //if (!IsVector3InArea(a, transform.position - new Vector3(loadTolerance, 0, loadTolerance), transform.position + new Vector3(loadTolerance, 0, loadTolerance)) && loaded.Contains(a))
            if (!IsVector3InArea(transform.position, a, loadTolerance) && loadedMapDatas.Contains(a))
            {

                mapDataStreamers[a].Destroy();
                loadedMapDatas.Remove(a);
                mapDataStreamers.Remove(a);

            }

        }

    }

    public bool IsVector3InArea(Vector3 reference, Vector3 lowerBound, Vector3 upperBound)
    {

        return (reference.x >= lowerBound.x && reference.x <= upperBound.x && reference.z >= lowerBound.z && reference.z <= upperBound.z);

    }
    public bool IsVector3InArea(Vector3 reference, Vector3 point,float range)
    {

        return Vector3.Distance(reference,point) < range;

    }

}
