using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles various forms of timer-based data logging.
/// </summary>
public class Timer : DataItem<float> {

    private enum TimerStartingPoint
    {
        ApplicationStart,
        ComponentStart
    }

    /// <summary>
    /// Should the timer measure from when the application started or when this component started logging?
    /// </summary>
    [Tooltip("Should the timer measure from when the application started or when the logging started?")]
    [SerializeField]
    private TimerStartingPoint startFrom = TimerStartingPoint.ApplicationStart;

    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}

    public override void OnDataLoggingRequested()
    {
        if (startFrom == TimerStartingPoint.ApplicationStart)
        {
            value = Time.time;
        }
        else
        {
            value = Time.time - startTime;
        }
    }
}
