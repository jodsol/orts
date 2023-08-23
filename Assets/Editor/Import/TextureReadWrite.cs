using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureReadWrite : AssetPostprocessor
{

    void OnPostprocessTexture(Texture2D texture)
    {

        //string assetPath = AssetDatabase.GetAssetPath(texture);
        //TextureImporter tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        //tImporter = assetImporter as TextureImporter;
        //if (tImporter != null)
        //{
        //    tImporter.textureType = TextureImporterType.Default;
        //
        //    tImporter.isReadable = true;
        //
        //    AssetDatabase.ImportAsset(assetPath);
        //    AssetDatabase.Refresh();
        //}

    }

}
