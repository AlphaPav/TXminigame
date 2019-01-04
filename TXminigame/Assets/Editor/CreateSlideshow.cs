using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateSlideshow : Editor
{
    private static GameObject m_slideshowPrefab = null;

    private static GameObject m_canvas = null;

    [MenuItem("GameObject/UI/Slideshow")]
    static void CreateSlideshowUI(MenuCommand menuCommand)
    {
        if (null == m_slideshowPrefab)
        {
            m_slideshowPrefab = Resources.Load<GameObject>("Slideshow");
            if (null == m_slideshowPrefab)
            {
                Debug.LogError("Prefab Slideshow is null");
                return;
            }
        }

        m_canvas = menuCommand.context as GameObject;
        if (m_canvas == null || m_canvas.GetComponentInParent<Canvas>() == null)
        {
            m_canvas = GetOrCreateCanvasGameObject();
        }
        
        GameObject go = GameObject.Instantiate(m_slideshowPrefab, m_canvas.transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "Slideshow";
        Selection.activeGameObject = go;
    }

    static public GameObject GetOrCreateCanvasGameObject()
    {
        GameObject selectedGo = Selection.activeGameObject;
        
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;
        
        canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;
        
        return CreateCanvas();
    }

    public static GameObject CreateCanvas()
    {
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);
        
        CreateEventSystem();
        return root;
    }

    public static void CreateEventSystem()
    {
        var esys = Object.FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, null);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }
    }
}
