
using UnityEngine;
using UnityEditor;
using System.IO;

public class FurniturePreviewGenerator : EditorWindow
{
    private FurniturePlacer placer;
    private int currentIndex;
    private bool generating;
    private string outputFolder = "Assets/FurniturePreviews";

    [MenuItem("Tools/Furniture/Generate Prefab Previews")]
    public static void ShowWindow()
    {
        GetWindow<FurniturePreviewGenerator>("Furniture Previews");
    }

    private void OnGUI()
    {
        GUILayout.Label("Furniture Preview Generator", EditorStyles.boldLabel);

        placer = (FurniturePlacer)EditorGUILayout.ObjectField(
            "Furniture Placer",
            placer,
            typeof(FurniturePlacer),
            true
        );

        if (!generating && GUILayout.Button("Generate Previews"))
        {
            if (placer == null || placer.furniturePrefabs == null)
            {
                Debug.LogError("Assign a FurniturePlacer first.");
                return;
            }

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            currentIndex = 0;
            generating = true;
            EditorApplication.update += GenerateNextPreview;
        }

        if (generating)
        {
            EditorGUILayout.LabelField(
                "Generating: " + currentIndex + " / " +
                (placer != null ? placer.furniturePrefabs.Length : 0)
            );
        }
    }

    private void GenerateNextPreview()
    {
        if (placer == null ||
            currentIndex >= placer.furniturePrefabs.Length)
        {
            FinishGeneration();
            return;
        }

        GameObject prefab = placer.furniturePrefabs[currentIndex];

        if (prefab == null)
        {
            currentIndex++;
            return;
        }

        Texture2D preview = AssetPreview.GetAssetPreview(prefab);

        if (preview == null)
        {
            Repaint();
            return;
        }

        string path = AssetDatabase.GetAssetPath(prefab);
        string guid = AssetDatabase.AssetPathToGUID(path);

        string fileName = prefab.name + "_" + guid + ".png";
        string fullPath = Path.Combine(outputFolder, fileName);

        File.WriteAllBytes(fullPath, preview.EncodeToPNG());

        currentIndex++;
        Repaint();
    }

    private void FinishGeneration()
    {
        EditorApplication.update -= GenerateNextPreview;

        generating = false;

        AssetDatabase.Refresh();

        Debug.Log("Furniture previews generated successfully!");

        Repaint();
    }

    private void OnDisable()
    {
        EditorApplication.update -= GenerateNextPreview;
    }
}
