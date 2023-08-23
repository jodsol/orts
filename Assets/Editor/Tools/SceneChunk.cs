using NUnit;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChunk : EditorWindow
{

    float xStart, yStart, step,xEnd,yEnd;
    GameObject environment;

    [MenuItem("Scene Chunk",menuItem ="Tools/Scene Chunk")]
    public static void ShowWindow()
    {
        
        GetWindow<SceneChunk>();

    }

    private void OnGUI()
    {

        EditorGUILayout.LabelField("Position of the bottom left corner :");
        EditorGUILayout.Space(2);
        xStart = EditorGUILayout.FloatField("x start : ",xStart);
        yStart = EditorGUILayout.FloatField("y start : ", yStart);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Position of the top right corner:");
        EditorGUILayout.Space(2);
        xEnd = EditorGUILayout.FloatField("x end : ", xEnd);
        yEnd = EditorGUILayout.FloatField("y end : ", yEnd);
        EditorGUILayout.Space(10);
        step = EditorGUILayout.FloatField("step : ", step);
        EditorGUILayout.Space(10);
        environment = (GameObject)EditorGUILayout.ObjectField(environment, typeof(GameObject), true);

        if(GUILayout.Button("Chunk the map"))
        {

            PlaceObjectsIntoChunks();

        }
        if (GUILayout.Button("Create Map Data"))
        {

            CreateMapDatas();

        }
        if (GUILayout.Button("Create Scene"))
        {

            CreateScenes();

        }

    }

    private void PlaceObjectsIntoChunks()
    {

        Transform[] childs;

        for(float x = xStart; x<= xEnd; x += step)
        {

            for (float y = yStart; y <= yEnd; y += step)
            {

                GameObject parentMap = new GameObject("MapX" + x.ToString() + "Y" + y.ToString());
                parentMap.transform.position = new Vector3(x, 0, y);
                childs = environment.GetComponentsInChildren<Transform>();
                for (int j = 0;j < childs.Length;j++)
                {

                    MeshRenderer temp;
                    if(childs[j].gameObject != environment && childs[j].TryGetComponent<MeshRenderer>(out temp))
                    {

                        if (IsVector3InArea(childs[j].position, new Vector3(x - step / 2f,0, y - step / 2), new Vector3(x + step / 2f,0, y + step / 2)))
                            childs[j].parent = parentMap.transform;

                    }

                }

            }

        }

    }

    public bool IsVector3InArea(Vector3 reference,Vector3 lowerBound,Vector3 upperBound)
    {

        return (reference.x >= lowerBound.x && reference.x <= upperBound.x && reference.z >= lowerBound.z && reference.z <= upperBound.z);

    }

    private static void CreateMapDatas()
    {

        foreach(GameObject a in Selection.gameObjects)
        {

            MapData mapData = ScriptableObject.CreateInstance<MapData>();
            string fileName = a.name;
            MapData.RecordObjects(mapData, a);
            AssetDatabase.CreateAsset(mapData, "Assets/Map data/" + fileName + ".asset");
            AssetDatabase.SaveAssets();

        }

    }

    private static void CreateScenes()
    {

        MapData[] ss= new MapData[Selection.objects.Length];
        for (int j = 0; j < ss.Length; j++)
            ss[j] = (MapData)Selection.objects[j];

        foreach(MapData a in ss)
        {

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            scene.name = a.name;
            //GameObject mapDataStreamer = new GameObject();
            // mapDataStreamer.name = "Map Data Streamer";
            //MapDataStreamer temp = mapDataStreamer.AddComponent<MapDataStreamer>();
            //temp.transform.position = Vector3.zero;
            //temp.environment = new GameObject();
            //temp.environment.transform.position = Vector3.zero;
            //temp.environment.name = "Environment";
            //temp.mapData = "Assets/Map Data/" + a.name + ".asset";
            a.InstantiateMapObjects();
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/Map/" + a.name + ".unity");

        }

    }

}
