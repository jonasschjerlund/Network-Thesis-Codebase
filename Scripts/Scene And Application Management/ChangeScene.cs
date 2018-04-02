using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Simple single-key manager for scene change operations locally and across the network. 
/// If the local connection is not the server, key press quits the application.
/// </summary>
public class ChangeScene : NetworkBehaviour {

    /// <summary>
    /// Designated key used by this component.
    /// </summary>
    [SerializeField]
    [Tooltip("Key used by this component.")]
    private KeyCode key = KeyCode.Escape;

    /// <summary>
    /// Target scene for a scene change.
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private string targetScene;

    /// <summary>
    /// The application will exit when the designated key is pressed.
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private bool quitApplication;

    /// <summary>
    /// The application will change to a designated scene when a key is pressed.
    /// </summary>
    [SerializeField]
    [HideInInspector]
    private bool changeScene;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (quitApplication)
            {
                Application.Quit();
            }
            else
            {
                if (!isServer)
                {
                    Application.Quit();
                }
                else
                {
                    NetworkManager.singleton.ServerChangeScene(targetScene);
                }
            }
        }
    }


/* #if UNITY_EDITOR
    // Custom inspector editor for this component that only gets compiled in the Unity editor (i.e. not included in deployed builds).
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(ChangeScene))]
    public class CloseApplicationOnKeyPressedEditor : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty quitApplicationProp;
        UnityEditor.SerializedProperty targetSceneProp;
        UnityEditor.SerializedProperty changeSceneProp;

        void OnEnable()
        {
            quitApplicationProp = serializedObject.FindProperty("quitApplication");
            targetSceneProp = serializedObject.FindProperty("targetScene");
            changeSceneProp = serializedObject.FindProperty("changeScene");

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            UnityEditor.EditorGUILayout.Space();
            UnityEditor.EditorGUILayout.LabelField("Functionalities", UnityEditor.EditorStyles.boldLabel);

            ChangeScene changeScene = target as ChangeScene;

            GUIContent content = new GUIContent("Quit Application",
                "Pressing " + changeScene.key + " will quit the application.");
            
            if (!changeSceneProp.boolValue)
            {
                UnityEditor.EditorGUI.BeginDisabledGroup(changeScene.isClient && !changeScene.isServer);
                quitApplicationProp.boolValue = UnityEditor.EditorGUILayout.Toggle(content, quitApplicationProp.boolValue);
                UnityEditor.EditorGUI.EndDisabledGroup();


                if (changeScene.isClient && !changeScene.isServer)
                {
                    quitApplicationProp.boolValue = true;
                    UnityEditor.EditorGUILayout.HelpBox("This connection does not own the server. Quitting application is mandatory.", UnityEditor.MessageType.Warning, true);
                }
            }

            if (!quitApplicationProp.boolValue)
            {
                content = new GUIContent("Change scene",
                    "The application will change to a designated scene when a key is pressed.");
                changeSceneProp.boolValue = UnityEditor.EditorGUILayout.Toggle(content, changeSceneProp.boolValue);

                if (changeSceneProp.boolValue)
                {
                    UnityEditor.EditorGUI.indentLevel++;

                    content = new GUIContent("Target Scene",
                        "Name of the scene to change to.");
                    targetSceneProp.stringValue = UnityEditor.EditorGUILayout.TextField(content, targetSceneProp.stringValue);

                    UnityEditor.EditorGUILayout.HelpBox("NB! Requires server privileges to cause a scene change.", UnityEditor.MessageType.Warning, false);
                    UnityEditor.EditorGUI.indentLevel--;
                }
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif*/
}
