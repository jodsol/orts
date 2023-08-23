using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class ModelImport : AssetPostprocessor
{

    void OnPreprocessModel()
    {

        ModelImporter modelImporter = (ModelImporter)assetImporter;
        modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
        modelImporter.materialLocation = ModelImporterMaterialLocation.InPrefab;
        modelImporter.normalCalculationMode = ModelImporterNormalCalculationMode.AreaAndAngleWeighted;
        modelImporter.importNormals = ModelImporterNormals.Calculate;

    }

}
