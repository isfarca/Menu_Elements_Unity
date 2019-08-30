using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MenuObject))] public class MenuPropertyDrawer : PropertyDrawer
{
    /// <summary>
    /// Draw the property inside the given rect
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var goRect = new Rect(position.x, position.y, position.width, position.height);

        var prefab = property.FindPropertyRelative("prefab");
        var go = EditorGUI.ObjectField(goRect, prefab.objectReferenceValue, typeof(GameObject), false) as GameObject;

        if (go != null && go != prefab.objectReferenceValue)
        {
            var component = go.GetComponent<Menu>();

            if (component != null)
            {
                prefab.objectReferenceValue = go;
                property.FindPropertyRelative("type").stringValue = component.GetType().ToString();
            }
            else
                EditorUtility.DisplayDialog("No suitable menu!", "The prefab does not have the corret menu script! ", "Bad...");
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}