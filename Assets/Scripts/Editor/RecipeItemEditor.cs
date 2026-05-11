using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RecipeItem))]
public class RecipeItemEditor : Editor
{
    private SerializedProperty _itemNameProperty;
    private SerializedProperty _itemSpriteProperty;
    private SerializedProperty _itemValueProperty;

    private void OnEnable()
    {
        _itemNameProperty = serializedObject.FindProperty("itemName");
        _itemSpriteProperty = serializedObject.FindProperty("itemSprite");
        _itemValueProperty = serializedObject.FindProperty("itemValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        // Clickable sprite preview
        EditorGUILayout.BeginVertical(GUILayout.Width(80));
        EditorGUILayout.LabelField("Sprite", EditorStyles.boldLabel, GUILayout.Width(80));

        Rect spriteRect = GUILayoutUtility.GetRect(80, 80, GUILayout.Width(80), GUILayout.Height(80));

        Sprite currentSprite = _itemSpriteProperty.objectReferenceValue as Sprite;

        if (currentSprite != null)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(currentSprite);
            if (texture != null)
            {
                GUI.DrawTexture(spriteRect, texture, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(spriteRect, new Color(0.2f, 0.2f, 0.2f));
                GUI.Label(spriteRect, "Loading...", EditorStyles.centeredGreyMiniLabel);
            }
        }
        else
        {
            EditorGUI.DrawRect(spriteRect, new Color(0.2f, 0.2f, 0.2f));
            GUI.Label(spriteRect, "None", EditorStyles.centeredGreyMiniLabel);
        }

        // Make sprite clickable
        if (Event.current.type == EventType.MouseDown && spriteRect.Contains(Event.current.mousePosition))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(currentSprite, false, "", EditorGUIUtility.GetControlID(FocusType.Passive));
            Event.current.Use();
        }

        // Handle object picker result
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            _itemSpriteProperty.objectReferenceValue = EditorGUIUtility.GetObjectPickerObject() as Sprite;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // Fields
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(_itemNameProperty, new GUIContent("Name"));

        EditorGUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value", GUILayout.Width(50));
        _itemValueProperty.intValue = EditorGUILayout.IntSlider(_itemValueProperty.intValue, 0, 1000);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        // Small sprite field as fallback
        EditorGUILayout.PropertyField(_itemSpriteProperty, new GUIContent("Sprite Reference"));

        serializedObject.ApplyModifiedProperties();
    }
}
