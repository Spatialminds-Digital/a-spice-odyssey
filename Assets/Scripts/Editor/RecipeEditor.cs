using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Recipe))]
public class RecipeEditor : Editor
{
    private SerializedProperty _recipeItemsProperty;

    private void OnEnable()
    {
        _recipeItemsProperty = serializedObject.FindProperty("recipeItems");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Recipe Items", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        HashSet<RecipeItem> usedItems = new HashSet<RecipeItem>();

        for (int i = 0; i < _recipeItemsProperty.arraySize; i++)
        {
            SerializedProperty element = _recipeItemsProperty.GetArrayElementAtIndex(i);
            SerializedProperty itemProperty = element.FindPropertyRelative("item");
            SerializedProperty countProperty = element.FindPropertyRelative("count");

            RecipeItem currentItem = itemProperty.objectReferenceValue as RecipeItem;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();

            // Sprite preview
            if (currentItem != null && currentItem.itemSprite != null)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(currentItem.itemSprite);
                if (texture != null)
                {
                    GUILayout.Label(texture, GUILayout.Width(50), GUILayout.Height(50));
                }
                else
                {
                    GUILayout.Label(GUIContent.none, GUILayout.Width(50), GUILayout.Height(50));
                }
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(50), GUILayout.Height(50));
            }

            EditorGUILayout.BeginVertical();

            // Item field
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(itemProperty, new GUIContent("Item"));
            if (EditorGUI.EndChangeCheck())
            {
                RecipeItem newItem = itemProperty.objectReferenceValue as RecipeItem;
                if (newItem != null && usedItems.Contains(newItem))
                {
                    itemProperty.objectReferenceValue = null;
                    Debug.LogWarning("This item is already in the recipe!");
                }
            }

            // Count field with nice slider
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Count", GUILayout.Width(50));
            countProperty.intValue = EditorGUILayout.IntSlider(countProperty.intValue, 1, 99);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            // Remove button
            if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(50)))
            {
                _recipeItemsProperty.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (currentItem != null)
            {
                usedItems.Add(currentItem);
            }

            EditorGUILayout.Space(2);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Add Recipe Item", GUILayout.Height(30)))
        {
            _recipeItemsProperty.InsertArrayElementAtIndex(_recipeItemsProperty.arraySize);
            SerializedProperty newElement = _recipeItemsProperty.GetArrayElementAtIndex(_recipeItemsProperty.arraySize - 1);
            newElement.FindPropertyRelative("item").objectReferenceValue = null;
            newElement.FindPropertyRelative("count").intValue = 1;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
