using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSystemController : SingletonMonoBehaviour<DataSystemController> {

    /// <summary>
    /// Different value separation delimiters.
    /// </summary>
    public enum DelimiterType
    {
        Semicolon = ';',
        Comma = ',',
        Tab = '\t'
    }

    /// <summary>
    /// Should this data logging setup be persistent across all scenes?
    /// </summary>
    [Tooltip("Make this data logging setup persistent across all scenes.")]
    [SerializeField]
    private bool dontDestroyOnLoad = true;

    /// <summary>
    /// Name of the current user. This will be used for the file naming convention of the data sets.
    /// </summary>
    [Tooltip("Name of the current user. This will be used for the file naming convention of the data sets.")]
    public string Username = "Enter name here";

    [Tooltip("Enclose the logged values in all data sets in double quotes.")]
    public bool EncloseValuesInQuotes = false;
    
    /// <summary>
    /// Use this delimiter to separate logged values in all data sets.
    /// </summary>
    [Tooltip("Use this delimiter to separate logged values in all data sets.")]
    public DelimiterType Delimiter = DelimiterType.Semicolon;

    // Use this for initialization
    void Start () {

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
	}
	
    /// <summary>
    /// Checks if any data logging children match the provided name, 
    /// and stops their logging by destroying them.
    /// </summary>
    /// <param name="name">Name of the game object the logging system is attached to.</param>
    public void StopLogging(string name)
    {
        foreach (Dataset dataset in transform.GetComponentsInChildren<Dataset>())
        {
            if (dataset.gameObject.name == name)
            {
                dataset.Active = false;
            }
        }
    }

    /// <summary>
    /// Stops a DataLogger by setting it to inactive.
    /// </summary>
    /// <param name="dataset">DataLogger to stop.</param>
    public void StopLogging(Dataset dataset)
    {
        dataset.Active = false;
    }

#if UNITY_EDITOR
    // Custom inspector editor for this component that only gets compiled in the Unity editor (i.e. not included in deployed builds).
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(DataSystemController))]
    public class DataLoggingControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DataSystemController dataLoggingController = target as DataSystemController;

            if (dataLoggingController.Delimiter == DelimiterType.Tab)
            {
                UnityEditor.EditorGUILayout.HelpBox("This delimiter will create .tsv data files.", UnityEditor.MessageType.Info, true);
            }
            else
            {
                UnityEditor.EditorGUILayout.HelpBox("This delimiter will create .csv data files.", UnityEditor.MessageType.Info, true);
            }
        }
    }
#endif

}
