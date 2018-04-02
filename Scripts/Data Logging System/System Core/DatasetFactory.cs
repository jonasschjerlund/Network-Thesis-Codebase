using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Factory for Dataset game object prefabs.
/// </summary>
[RequireComponent(typeof(DataSystemController))]
public class DatasetFactory : SingletonMonoBehaviour<DatasetFactory> {

    /// <summary>
    /// Prefabs of complete datasets that the factory can create.
    /// </summary>
    [Tooltip("Prefabs of complete datasets that the factory can create.")]
    public GameObject[] DatasetPrefabs;

    private GameObject datasetContainer;

	// Use this for initialization
	void Start () {
        if (transform.Find("Data Sets") != null)
        {
            datasetContainer = transform.Find("Data Sets").gameObject;
        }
        else
        {
            datasetContainer = Instantiate(new GameObject());
            datasetContainer.name = "Data Sets";
            datasetContainer.transform.parent = transform;
        }
        MoveMisplacedDatasets();
	}

    /// <summary>
    /// Finds all datasets, and if their parent is not the designated dataset container,
    /// sets their parent to be the designated dataset container.
    /// </summary>
    private void MoveMisplacedDatasets()
    {
        foreach (Dataset dataset in FindObjectsOfType<Dataset>())
        {
            if (dataset.transform.parent != datasetContainer)
            {
                dataset.transform.parent = datasetContainer.transform;
            }
        }
    }
	
    /// <summary>
    /// Creates a dataset of the provided name, if that dataset exists in the
    /// factory's array of prefabs.
    /// </summary>
    /// <param name="datasetName">Name of dataset to create.</param>
    public void CreateDataset(string datasetName)
    {
        foreach (GameObject dataset in DatasetPrefabs)
        {
            if (dataset.name == datasetName)
            {
                Instantiate(dataset, datasetContainer.transform);
                return;
            }
        }
        Debug.LogWarning("Tried to create a dataset that is not contained in the dataset prefabs of the dataset factory.");
    }

    /// <summary>
    /// Creates a dataset of the provided game object type, if that dataset exists in the
    /// factory's array of prefabs.
    /// </summary>
    /// <param name="dataset"></param>
    public void CreateDataset(GameObject dataset)
    {
        if (DatasetPrefabs.Contains(dataset))
        {
            Instantiate(dataset, datasetContainer.transform);
            return;
        }
        Debug.LogWarning("Tried to create a dataset that is not contained in the dataset prefabs of the dataset factory.");
    }

    /// <summary>
    /// Stops every active dataset whose name matches the provided string by setting its Active bool to false.
    /// </summary>
    /// <param name="datasetName">Name of datasets to stop.</param>
    public void StopDatasetLogging(string datasetName)
    {
        foreach (Dataset dataset in GetComponentsInChildren<Dataset>())
        {
            if (dataset.DatasetName == datasetName)
            {
                dataset.Active = false;
            }
        }
    }

    /// <summary>
    /// Stops logging the dataset by setting its Active bool to false.
    /// </summary>
    /// <param name="dataset">Dataset to stop.</param>
    public void StopDataSetLogging(Dataset dataset)
    {
        dataset.Active = false;
    }

    /// <summary>
    /// Stops the dataset contained in the provided game object by setting its Active bool to false.
    /// </summary>
    /// <param name="datasetGameObject">Game object containing a dataset to false.</param>
    public void StopDataSetLogging(GameObject datasetGameObject)
    {
        if (datasetGameObject.GetComponent<Dataset>() != null)
        {
            datasetGameObject.GetComponent<Dataset>().Active = false;
        }
        else
        {
            Debug.LogWarning("No dataset attached to the provided game object.");
        }
    }
}
