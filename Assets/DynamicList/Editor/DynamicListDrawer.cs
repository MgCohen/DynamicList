using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(DynamicList<>))]
public class DynamicListDrawer : PropertyDrawer
{
    private int m_dropdownIndex = 0;
    private List<Type> m_typeList;
    private string[] m_typeNames;
    private ReorderableList m_reordList;
    private DynamicList m_instance;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return m_reordList != null ? m_reordList.GetHeight() + 5 : 5;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DrawList(position, property, label);

        Rect dropdownRect = new Rect(position.x, position.y + m_reordList.GetHeight() - 19, position.width - 75, 20);
        m_dropdownIndex = EditorGUI.Popup(dropdownRect, m_dropdownIndex, m_typeNames);
    }


    public void DrawList(Rect position, SerializedProperty property, GUIContent label)
    {
        var array = property.FindPropertyRelative("list");
        var length = array.arraySize;

        if (m_reordList == null)
        {
            m_instance = fieldInfo.GetValue(property.serializedObject.targetObject) as DynamicList;
            m_reordList = new ReorderableList(array.serializedObject, array, true, true, true, true);
            Type listType = m_instance.ListType();
             m_typeList = GetAllDerivedTypes(listType, true).ToList();
            m_typeNames = m_typeList.Select(t => t.Name).ToArray();
        }

        m_reordList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, property.displayName);
        };

        m_reordList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.x += 15f;
                rect.width -= 15f;
                SerializedProperty element = array.GetArrayElementAtIndex(index);
                Type type = m_instance.GetIndexType(index);
                EditorGUI.PropertyField(rect, element, new GUIContent(type.Name), true);
            };

        m_reordList.elementHeightCallback = (index) =>
        {
            int propertyCount = array.GetArrayElementAtIndex(index).CountInProperty();
            return (20f * propertyCount) + 5f;
        };

        m_reordList.onAddCallback = (list) =>
        {
            int last = m_reordList.serializedProperty.arraySize;
            m_reordList.serializedProperty.InsertArrayElementAtIndex(last);

            SerializedProperty lastProp = m_reordList.serializedProperty.GetArrayElementAtIndex(last);
            lastProp.managedReferenceValue = Activator.CreateInstance(m_typeList[m_dropdownIndex]);
        };


        m_reordList.DoList(position);
        array.serializedObject.ApplyModifiedProperties();
    }

    public Type[] GetAllDerivedTypes(Type type, bool includesSource = false)
    {
        var domain = AppDomain.CurrentDomain;
        var result = new List<System.Type>();
        var assemblies = domain.GetAssemblies();

        if(includesSource && !type.IsAbstract)
        {
            result.Add(type);
        }

        foreach (var assembly in assemblies)
        {
            var aTypes = assembly.GetTypes();
            foreach (var aType in aTypes)
            {
                if (aType.IsSubclassOf(type) && !aType.IsAbstract)
                {
                    result.Add(aType);
                }
            }
        }
        return result.ToArray();
    }

    private int CompareTypesNames(Type a, Type b)
    {
        return a.Name.CompareTo(b.Name);
    }
}
