using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Library))]
public class LibraryEditor : Editor
{
    private EditorLibrary editorLibrary;

    public override void OnInspectorGUI()
    {
        // With no use of base.OnInspectorGUI() starting the inspector of script from scretch
        editorLibrary = CreateInstance<EditorLibrary>();

        // Accesing the Library script object type declaration with System.Type
        Type typeLibrary = (typeof(Library));

        // Get the methods of Library class with BindFlags filter
        MethodInfo[] methodArray = typeLibrary.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        // Add a labelfield to show the count of the methods
        EditorGUILayout.LabelField("Method Count : " + methodArray.Length.ToString(), EditorStyles.boldLabel);

        // Show all the method's in the script as string
        GUILayout.Label(editorLibrary.MethodInfoArrayToString(methodArray));

        // Title
        string infoText = "This is a script contains methods that can be used everywhere in the game.";

        /**
         * Show the infoText with ITALIC font style
         * 
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Italic
        };

        GUILayout.Label(infoText, labelStyle);
        */

        EditorGUILayout.HelpBox(infoText, MessageType.Info);
    }
}
