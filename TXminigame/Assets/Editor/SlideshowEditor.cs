using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Advertisements;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(Slideshow), true)]
    public class SlideshowEditor : Editor
    {
        SerializedProperty m_movement;
        SerializedProperty m_content;
        SerializedProperty m_lastPageButton;
        SerializedProperty m_nextPageButton;
        SerializedProperty m_showTime;
        SerializedProperty m_pageToggleGroup;
        SerializedProperty m_onValueChanged;
        SerializedProperty m_allowDrag;
        SerializedProperty m_autoSlide;

        protected virtual void OnEnable()
        {
            m_movement = serializedObject.FindProperty("m_movement");
            m_content = serializedObject.FindProperty("m_content");
            m_lastPageButton = serializedObject.FindProperty("m_lastPageButton");
            m_nextPageButton = serializedObject.FindProperty("m_nextPageButton");
            m_showTime = serializedObject.FindProperty("m_showTime");
            m_pageToggleGroup = serializedObject.FindProperty("m_pageToggleGroup");
            m_onValueChanged = serializedObject.FindProperty("m_onValueChanged");
            m_allowDrag = serializedObject.FindProperty("m_allowDrag");
            m_autoSlide = serializedObject.FindProperty("m_autoSlide");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_movement);
            EditorGUILayout.PropertyField(m_content);
            EditorGUILayout.PropertyField(m_lastPageButton);
            EditorGUILayout.PropertyField(m_nextPageButton);
            EditorGUILayout.PropertyField(m_allowDrag);
            EditorGUILayout.PropertyField(m_autoSlide);
            EditorGUILayout.PropertyField(m_showTime);
            EditorGUILayout.PropertyField(m_pageToggleGroup);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_onValueChanged);

            //不加这句代码，在编辑模式下，无法将物体拖拽赋值
            serializedObject.ApplyModifiedProperties();
        }
    }
}
