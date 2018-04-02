using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a variable in a table, given a specific datatype.
/// </summary>
[RequireComponent(typeof(DatasetMediator))]
public abstract class DataItem<TDataType> : MonoBehaviour, IDataItem {

    /// <summary>
    /// The current value of this data item. 
    /// This represents the data that gets logged.
    /// </summary>
    protected TDataType value;

    /// <summary>
    /// Returns the value of this component as a string. 
    /// Useful as a virtual property in case we want to 
    /// further treat the value before converting it to a string.
    /// </summary>
    public virtual string ValueAsString
    {
        get
        {
            return value.ToString();
        }
    }

    /// <summary>
    /// Property that returns the data logger responsible for this data item.
    /// </summary>
    public Dataset Dataset
    {
        get
        {
            return GetComponent<DatasetMediator>().Dataset;
        }
    }

    /// <summary>
    /// Gets the current value of this data item. Useful as a virtual property 
    /// in case we want to further treat the data before returning it.
    /// </summary>
    public virtual TDataType Value
    {
        get
        {
            return value;
        }
    }

    // Unity method that gets called whenever the component is enabled
    public virtual void OnEnable()
    {
        Dataset.OnDataLoggingRequested += OnDataLoggingRequested;
    }

    // Unity method that gets called whenever the component is disabled
    public virtual void OnDisable()
    {
        Dataset.OnDataLoggingRequested -= OnDataLoggingRequested;
    }

    /// <summary>
    /// Invoked when a data logging request from this item's data set is received. 
    /// It is invoked before the value is logged. 
    /// See also:
    /// <seealso cref="Dataset.LogRow"/>
    /// </summary>
    public virtual void OnDataLoggingRequested() { }
}