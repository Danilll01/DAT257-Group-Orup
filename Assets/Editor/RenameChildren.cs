using UnityEngine;
using UnityEditor;
using System.Linq;

public class RenameChildren : EditorWindow
{
    // Size of window
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    
    // Which bar nr to rename to
    private int barNr;
    [MenuItem("GameObject/Rename bar indexes")]

    public static void ShowWindow()
    {
        // Sets up window
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {
        barNr = EditorGUILayout.IntField("Bar nr", barNr);

        // On button click rename all selected objects with specified bar nr
        if (GUILayout.Button("Rename bars"))
        {
            // Get selection of objects
            GameObject[] selectedObjects = Selection.gameObjects;

            // Loop trough all selected objects
            for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
            {
                // Get name of object
                string childrenName = selectedObjects[objectI].name;

                // Remove "(1)" from the end of the string
                childrenName = childrenName.Split(" ")[0];

                // Removes "BarX" from the string
                string[] childrenNameArrWithoutBar = childrenName.Split("-").Skip(1).ToArray();

                // Concat string with "-" as seperator
                childrenName = string.Join("-", childrenNameArrWithoutBar);

                // Rename object to new name
                selectedObjects[objectI].name = $"Bar{barNr}-{childrenName}";
            }
        }
    }
}