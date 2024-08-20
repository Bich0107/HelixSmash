using UnityEditor;
using UnityEngine;
using System.IO;

public class ModelImporter : EditorWindow
{
    private string folderPath = "Assets/Models/Models"; // Default path to the models folder
    private GameObject parentObject;

    [MenuItem("Tools/Import Models and Create GameObjects")]
    public static void ShowWindow()
    {
        GetWindow<ModelImporter>("Model Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import Models and Create GameObjects", EditorStyles.boldLabel);

        // Folder path field
        folderPath = EditorGUILayout.TextField("Models Folder Path", folderPath);

        // Parent GameObject field
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent GameObject (Optional)", parentObject, typeof(GameObject), true);

        if (GUILayout.Button("Import Models"))
        {
            ImportModels();
        }
    }

    private void ImportModels()
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Folder path is empty. Please specify a valid path.");
            return;
        }

        string[] modelPaths = Directory.GetFiles(folderPath, "*.obj", SearchOption.AllDirectories);

        foreach (string modelPath in modelPaths)
        {
            string assetPath = modelPath.Replace(Application.dataPath, "Assets");
            GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (modelPrefab != null)
            {
                // Create an empty GameObject
                GameObject emptyGO = new GameObject(Path.GetFileNameWithoutExtension(assetPath));

                // Set the parent if specified
                if (parentObject != null)
                {
                    emptyGO.transform.SetParent(parentObject.transform);
                }

                // Instantiate the model as a child of the empty GameObject
                GameObject modelInstance = (GameObject)PrefabUtility.InstantiatePrefab(modelPrefab);
                modelInstance.transform.SetParent(emptyGO.transform);

                // Optionally, reset the position and rotation
                modelInstance.transform.localPosition = Vector3.zero;
                modelInstance.transform.localRotation = Quaternion.identity;

                // Mark the new GameObject as dirty to ensure it saves correctly
                EditorUtility.SetDirty(emptyGO);
            }
            else
            {
                Debug.LogWarning($"Failed to load model at path: {assetPath}");
            }
        }

        // Refresh the asset database to show changes
        AssetDatabase.Refresh();

        Debug.Log("Model import and GameObject creation complete.");
    }
}
