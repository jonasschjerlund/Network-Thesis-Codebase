using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Logs data into a .CSV file given a specific output scheme.
/// </summary>
public class Dataset : MonoBehaviour {

    /// <summary>
    /// Name of the dataset. This will be used for the file naming convention of the data set.
    /// </summary>
    [Tooltip("Name of the dataset. This will be used for the file naming convention of the data set.")]
    public string DatasetName = "Enter name of data set here";

    /// <summary>
    /// Drag game object with data item components here.
    /// </summary>
    [Tooltip("Drag game object with data item components here.")]
    [SerializeField]
    private DatasetMediator datasetMediator;

    /// <summary>
    /// Should data be logged?
    /// </summary>
    [Tooltip("Should data be logged?")]
    public bool Active;

    /// <summary>
    /// Should data logging be synchronized across components?
    /// </summary>
    [Tooltip("Should data logging be synchronized across components?")]
    [SerializeField]
    private bool synchronizeLogging;

    /// <summary>
    /// How often should data be logged (in seconds)?
    /// </summary>
    [Tooltip("How often should data be logged (in seconds)?")]
    [SerializeField]
    private float interval = 5f;

    /// <summary>
    /// The schema for the data to be logged.
    /// Each item corresponds to a column in the .CSV file.
    /// Gets populated in the Unity inspector.
    /// </summary>
    [Tooltip("The schema for the data to be logged. Each item corresponds to a column in the .CSV file. Gets populated here.")]
    [SerializeField]
    private string[] schema;

    public string Path { get; private set; }

    public delegate void OnDataLoggingRequestedDelegate();
    public event OnDataLoggingRequestedDelegate OnDataLoggingRequested;

    private IDataItem[] dataItems;
    private string formattedSchema;
    private char delimiter;

    void Awake()
    {
        // Check if there is a controller singleton; if not, shut down logging
        if (DataSystemController.Instance == null)
        {
            Debug.LogError("No data logging controller found. Dataset game object set to inactive.");
            gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {

        // Get delimiter as char
        delimiter = (char) DataSystemController.Instance.Delimiter;

        formattedSchema = string.Empty;
        
        PopulateSchema();

        // Determine file type based on delimiter type
        string fileType = string.Empty;
        if (DataSystemController.Instance.Delimiter == DataSystemController.DelimiterType.Tab)
        {
            fileType = ".tsv";
        }
        else
        {
            fileType = ".csv";
        }

        // Create directory info to standard "streaming assets" folder of application
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/StreamingAssets/");

        // Get user name and dataset name, format for file naming convention
        string formattedUsername = DataSystemController.Instance.Username.Replace(" ", "-").ToLower();
        string formattedDatasetName = DatasetName.Replace(" ", "-").ToLower();

        // Define file ID by counting previous files (+1)
        int fileID = directoryInfo.GetFiles("*" + fileType, SearchOption.TopDirectoryOnly).Where(file => file.Name.Contains(formattedUsername) && file.Name.Contains(formattedDatasetName)).Count() + 1;

        // Define file path based on directory info, scene name and user ID
        Path = directoryInfo + DatasetName.Replace(" ", "-").ToLower() + "-" + fileID + "-" + formattedUsername + fileType;

        File.WriteAllText(Path, formattedSchema + System.Environment.NewLine);

        // Start coroutine that handles synchronization of data logging
        StartCoroutine(SynchronizeLogging());
    }

    /// <summary>
    /// Gets the names of all of the data item components, preparing them as the schema
    /// for the logging.
    /// </summary>
    void PopulateSchema()
    {
        dataItems = datasetMediator.DataItems;
        schema = new string[dataItems.Length];

        int i = 0;
        foreach (IDataItem dataItem in dataItems)
        {
            string currentItemName = (dataItem as MonoBehaviour).GetType().ToString();
            schema[i] = currentItemName;
            formattedSchema += currentItemName + delimiter;
            i++;
        }
    }

    /// <summary>
    /// Log a row of data to a pre-designated output file. Use semicolons as delimiter.
    /// </summary>
    public void LogRow()
    {
        if (!Active)
        {
            Debug.LogError("Tried to log data while data logger component was set to inactive.");
            return;
        }

        if (OnDataLoggingRequested == null)
        {
            Debug.LogError("Nobody is subscribed to the data logging event. Check your data items.");
            return;
        }

        OnDataLoggingRequested.Invoke();
        
        string input = string.Empty;
        foreach (IDataItem dataItem in dataItems)
        {
            if (DataSystemController.Instance.EncloseValuesInQuotes)
            {
                input += "\"" + dataItem.ValueAsString + "\"" + delimiter;
            }
            else
            {
                input += dataItem.ValueAsString + delimiter;
            }
        }

        File.AppendAllText(Path, input + System.Environment.NewLine);
    }

    IEnumerator SynchronizeLogging()
    {
        while (true)
        {
            while (synchronizeLogging)
            {
                yield return new WaitForSeconds(interval);
                LogRow();
            }
            yield return new WaitUntil(() => synchronizeLogging);
        }
    }
}
