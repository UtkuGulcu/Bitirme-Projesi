using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EventTrackerWindow : EditorWindow
{
    private Vector2 scrollPosition = Vector2.zero;
    private string searchQuery = "";

    private class EventData
    {
        public GameEventSO gameEvent;
        public bool showEvent = false;
        public bool showSubscribers = false;
        public bool showPublishers = false;
        public List<GameObject> subscribers = new List<GameObject>();
        public List<GameObject> publishers = new List<GameObject>();
    }

    private List<EventData> eventDataList = new List<EventData>();

    [MenuItem("Window/Event Tracker")]
    public static void ShowWindow()
    {
        GetWindow<EventTrackerWindow>("Event Tracker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Event Subscribers and Publishers", EditorStyles.boldLabel);

        // Add search bar
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Push the search bar to the right
        searchQuery = EditorGUILayout.TextField(searchQuery, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();

        // Use BeginScrollView and EndScrollView to make the content scrollable
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Display each event with drawers for publishers and subscribers
        foreach (var eventData in eventDataList)
        {
            // Filter events based on the search query
            if (!string.IsNullOrEmpty(searchQuery) && !eventData.gameEvent.name.ToLower().Contains(searchQuery.ToLower()))
            {
                continue;
            }

            EditorGUILayout.BeginVertical("box");

            // Display event drawer
            eventData.showEvent = DrawCustomFoldout(eventData.showEvent, $"{eventData.gameEvent.name}", () => SelectObjectInProject(eventData.gameEvent));

            EditorGUILayout.Space();

            if (eventData.showEvent)
            {
                EditorGUI.indentLevel++;

                // Display publishers drawer if there are publishers
                if (eventData.publishers.Count > 0)
                {
                    eventData.showPublishers = DrawCustomFoldout(eventData.showPublishers, "Publishers", () => { });

                    EditorGUILayout.Space();

                    if (eventData.showPublishers)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var publisher in eventData.publishers)
                        {
                            DrawClickableLabel($"- {publisher.name}", () => SelectObjectInHierarchy(publisher));
                        }

                        EditorGUILayout.Space();

                        EditorGUI.indentLevel--;
                    }
                }

                // Display subscribers drawer if there are subscribers
                if (eventData.subscribers.Count > 0)
                {
                    eventData.showSubscribers = DrawCustomFoldout(eventData.showSubscribers, "Subscribers", () => { });

                    EditorGUILayout.Space();

                    if (eventData.showSubscribers)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var subscriber in eventData.subscribers)
                        {
                            DrawClickableLabel($"- {subscriber.name}", () => SelectObjectInHierarchy(subscriber));
                        }

                        EditorGUILayout.Space();

                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }




    private bool DrawCustomFoldout(bool foldout, string label, System.Action onClick = null)
    {
        Rect position = GUILayoutUtility.GetRect(16f, 16f);
    
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
        foldout = EditorGUI.Foldout(position, foldout, label, true, foldoutStyle);

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
        Event e = Event.current;

        if (position.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
        {
            onClick?.Invoke();
            e.Use();
        }

        return foldout;
    }


    private void DrawClickableLabel(string label, System.Action onClick)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label);
        EditorGUI.LabelField(position, new GUIContent(label), EditorStyles.label);

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
        Event e = Event.current;
        if (position.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
        {
            onClick?.Invoke();
            e.Use();
        }
    }

    private void OnEnable()
    {
        // Initialize the list of events and their data
        eventDataList.Clear();

        var guids = AssetDatabase.FindAssets("t:GameEventSO");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameEventSO gameEvent = AssetDatabase.LoadAssetAtPath<GameEventSO>(path);

            EventData eventData = new EventData
            {
                gameEvent = gameEvent
            };

            // Collect subscribers
            var gameEventListeners = Resources.FindObjectsOfTypeAll<GameEventListener>();
            foreach (var listener in gameEventListeners)
            {
                SerializedObject serializedListener = new SerializedObject(listener);
                SerializedProperty gameEventProperty = serializedListener.FindProperty("GameEvent");

                if (gameEventProperty.objectReferenceValue == gameEvent)
                {
                    eventData.subscribers.Add(listener.gameObject);
                }
            }

            // Collect publishers
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                var scripts = obj.GetComponents<MonoBehaviour>();
                foreach (var script in scripts)
                {
                    // Skip scripts of type GameEventListener
                    if (script is GameEventListener)
                    {
                        continue;
                    }

                    var scriptType = script.GetType();
                    var fields = scriptType.GetFields(
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

                    foreach (var field in fields)
                    {
                        if (field.FieldType == typeof(GameEventSO))
                        {
                            var value = field.GetValue(script) as GameEventSO;
                            if (value == gameEvent)
                            {
                                eventData.publishers.Add(obj);
                            }
                        }
                    }
                }
            }

            eventDataList.Add(eventData);
        }
    }

    private void SelectObjectInProject(Object obj)
    {
        Selection.activeObject = obj;
    }

    private void SelectObjectInHierarchy(GameObject obj)
    {
        Selection.activeGameObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
}
