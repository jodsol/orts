using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapData)), CanEditMultipleObjects]
public class MapDataInspector : Editor
{

    public GameObject parentMap;
    MapData mapData;

    public override void OnInspectorGUI()
    {

        parentMap = (GameObject)EditorGUILayout.ObjectField(parentMap, typeof(GameObject), true);
        mapData = (MapData)target;

        EditorUtility.SetDirty(target);
        EditorGUILayout.LabelField("Number of Object : " + mapData.mapObjects.Count.ToString());

        if (GUILayout.Button("Record Map Objects"))
        {

            foreach (Object a in targets)
            {

                mapData = (MapData)a;
                MapData.RecordObjects(mapData, parentMap);

            }

        }
        if (GUILayout.Button("Instantiate Map Objects"))
        {

            foreach (Object a in targets)
            {

                mapData = (MapData)a;

                foreach (MapObject b in mapData.mapObjects)
                {

                    mapData.InstantiateMapObject(b);

                }

            }

        }

    }

}
