using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * Esse código não foi feito pela GravLab!!!
 * Código se encontra no site:
 * https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
 */

namespace Base.Extensions.Attributes
{

#if UNITY_EDITOR
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#else
    public class ReadOnlyAttribute : Attribute
    {

    }
#endif
}
