using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that dynamically adjusts the Y-position of a game object
/// based on a user's height.
/// </summary>
public class DynamicHeightAdjuster : MonoBehaviour {

    [SerializeField]
    /// <summary>
    /// The offset in meters that will be added to the user's average height.
    /// </summary>
    private float offset = 0.35f;
    /// <summary>
    /// Should the user's height be adjusted with a set interval? 
    /// If disabled, the height will be set once, when the component starts.
    /// </summary>
    [SerializeField]
    private bool useInterval = true;
    /// <summary>
    /// How often in seconds the offset of this game object will be adjusted.
    /// </summary>
    [SerializeField]
    private float interval = 3.0f;

    private AverageHeight averageHeight;
    private float timer = 0;

    // Use this for initialization
    void Start() {
        if (GameObject.Find("Data Items").GetComponent<AverageHeight>())
        {
            averageHeight = GameObject.Find("Data Items").GetComponent<AverageHeight>();
            Debug.Log(averageHeight.Value);
        }

        if (averageHeight == null)
        {
            Debug.LogWarning("Could not find average height data item for the dynamic height adjuster!");
            return;
        }

        StartCoroutine(AdjustHeightWithInterval());

        if (!useInterval) SetHeight(offset);
    }

    /// <summary>
    /// Adjusts the height of this game object with a fixed interval. See 
    /// <see cref="SetHeight(float)"/>
    /// </summary>
    IEnumerator AdjustHeightWithInterval()
    {
        while (true)
        {
            while (useInterval)
            {
                SetHeight(offset);
                yield return new WaitForSeconds(Mathf.Abs(interval));
            }
            yield return new WaitUntil(() => useInterval);
        }
    }


    /// <summary>
    /// Changes the Y-position of this game object to the user's average height plus an offset.
    /// </summary>
    /// <param name="offset">Offset in meters.</param>
    public void SetHeight(float offset)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, averageHeight.Value + offset, transform.localPosition.z);
    }

}

#if UNITY_EDITOR
// Custom inspector editor for this component that only gets compiled in the Unity editor (i.e. not included in deployed builds).
[UnityEditor.CanEditMultipleObjects]
[UnityEditor.CustomEditor(typeof(DynamicHeightAdjuster))]
public class DynamicHeightAdjusterEditor : UnityEditor.Editor
{
    UnityEditor.SerializedProperty offsetProp;
    UnityEditor.SerializedProperty useIntervalProp;
    UnityEditor.SerializedProperty intervalProp;

    void OnEnable()
    {
        offsetProp = serializedObject.FindProperty("offset");
        useIntervalProp = serializedObject.FindProperty("useInterval");
        intervalProp = serializedObject.FindProperty("interval");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIContent content = new GUIContent("Offset",
            "The offset in meters that will be added to the user's average height.");
        offsetProp.floatValue = UnityEditor.EditorGUILayout.FloatField(content, offsetProp.floatValue);

        content = new GUIContent("Use Interval",
            "Should the user's height be adjusted with a set interval? If disabled, the height will be set once, when this component starts.");
        useIntervalProp.boolValue = UnityEditor.EditorGUILayout.Toggle(content, useIntervalProp.boolValue);

        if (useIntervalProp.boolValue)
        {
            UnityEditor.EditorGUI.indentLevel++;

            content = new GUIContent("Interval",
                "How often in seconds the offset of this game object will be adjusted.");
            intervalProp.floatValue = UnityEditor.EditorGUILayout.FloatField(content, Mathf.Abs(intervalProp.floatValue));

            UnityEditor.EditorGUI.indentLevel--;
        }
        else if (!Application.isPlaying)
        {
            UnityEditor.EditorGUILayout.HelpBox("The game object's height will be set once, when this component starts.", UnityEditor.MessageType.Info);
        }
        else if (Application.isPlaying)
        {
            UnityEditor.EditorGUILayout.HelpBox("The game object's height was set once, when this component started.", UnityEditor.MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif