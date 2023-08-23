using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

public class GeneralTools : Editor
{

    [MenuItem("Tools/General Tools/Change Asset Address to Asset Name")]
    public static void ChangeAddressToAssetName()
    {

        for (int j = 0; j < Selection.objects.Length; j++)
        {

            EditorUtility.DisplayProgressBar("Coverting asset adresses to asset name : " + ((float)j) / ((float)Selection.objects.Length), "Converting", ((float)j) / ((float)Selection.objects.Length));
            AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Selection.objects[j]))).address = Selection.objects[j].name;

        }

        EditorUtility.ClearProgressBar();

    }

    [MenuItem("Tools/General Tools/Add LOD")]
    public static void AddLod()
    {

        for (int j = 0; j < Selection.objects.Length; j++)
        {

            EditorUtility.DisplayProgressBar("Adding LOD Group to " + Selection.objects[j].name + " : " + ((float)j) / ((float)Selection.objects.Length), "Adding LOD Group", ((float)j) / ((float)Selection.objects.Length));
            LODGroup lod;
            if (((GameObject)Selection.objects[j]).TryGetComponent<LODGroup>(out lod))
            {


            }
            else
                lod = ((GameObject)Selection.objects[j]).AddComponent<LODGroup>();
            LOD[] lods = new LOD[1];
            Renderer[] renderers = { lod.GetComponent<Renderer>() };
            lods[0] = new LOD(0.02f, renderers);
            lod.SetLODs(lods);

        }

        EditorUtility.ClearProgressBar();

    }

    [MenuItem("Tools/General Tools/Vertex Count")]
    public static void PrintVertex()
    {

        Debug.Log(Selection.activeGameObject.GetComponent<MeshFilter>().mesh.vertexCount);

    }

}
