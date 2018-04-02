using UnityEngine;

/// <summary>
/// Contains a DataLogger object that other components attached to this entity can reference. 
/// <see cref="Dataset"/>
/// </summary>
public class DatasetMediator : MonoBehaviour {

    /// <summary>
    /// Points to the DataLogger component that the data items attached to this entity synchronize to.
    /// </summary>
    [Tooltip("Drag the DataLogger component that the data items attached to this entity synchronize to here.")]
    [SerializeField]
    private Dataset dataLogger;

    /// <summary>
    /// Points to the DataLogger component that the data items attached to this entity synchronize to.
    /// </summary>
    public Dataset Dataset
    {
        get
        {
            return dataLogger;
        }
        set
        {
            dataLogger = value;
        }
    }

    /// <summary>
    /// Points to the data items attached to this game object.
    /// </summary>
    public IDataItem[] DataItems
    {
        get
        {
            return GetComponents<IDataItem>();
        }
    }

    void Start()
    {
        if ( dataLogger == null)
        {
            Debug.LogWarning("Data Logger not assigned. Logging won't function.");
        }
    }
}

#if UNITY_EDITOR
// Custom inspector editor for this component that only gets compiled in the Unity editor (i.e. not included in deployed builds).
[UnityEditor.CanEditMultipleObjects]
[UnityEditor.CustomEditor(typeof(DatasetMediator))]
public class DataLoggerPointerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DatasetMediator dataLoggerPointer = target as DatasetMediator;

        if (dataLoggerPointer.Dataset == null)
        {
            UnityEditor.EditorGUILayout.HelpBox("Data Logger not assigned. Logging won't function.", UnityEditor.MessageType.Warning, true);
        }

        if (dataLoggerPointer.DataItems.Length == 0)
        {
            UnityEditor.EditorGUILayout.HelpBox("There are no components that implement the data item interface on this game object.", UnityEditor.MessageType.Warning, true);
        }
    }
}
#endif
