using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName ="New Map Data",menuName = "Map/Map Data"),Serializable]
public class MapData : ScriptableObject
{

    public List<MapObject> mapObjects;
    private void OnEnable()
    {

        if (mapObjects == null)
        {

            mapObjects = new List<MapObject>();

        }

    }

    private void Awake()
    {

        if(mapObjects == null)
        {

            mapObjects = new List<MapObject>();

        }

    }

    public void AddObject(MapObject mapObject)
    {

        mapObjects.Add(mapObject);

    }
#if UNITY_EDITOR
    public void InstantiateMapObjects()
    {

        foreach (MapObject a in mapObjects)
            InstantiateMapObject(a);

    }

    public void InstantiateMapObject(MapObject mapObject)
    {

        GameObject instantiatedMapObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Map/" + mapObject.objectName + ".prefab",typeof(GameObject)) as GameObject;
        if(instantiatedMapObject != null)
        {

            instantiatedMapObject = Instantiate(instantiatedMapObject);

            instantiatedMapObject.transform.position = mapObject.objectPosition;
            instantiatedMapObject.transform.rotation = mapObject.objectRotation;
            instantiatedMapObject.name = mapObject.objectName;

        }
        else
            Debug.Log("Object " + mapObject.objectName + " could not be found");

    }

#endif

    public static void RecordObjects(MapData mapData, GameObject parentMap)
    {

        Transform[] objects = parentMap.GetComponentsInChildren<Transform>();
        mapData.mapObjects.Clear();

        foreach (Transform a in objects)
        {

            if (a.name == parentMap.name)
                continue;

            MapObject temp = new MapObject(a);
            mapData.AddObject(temp);

        }

    }

}
