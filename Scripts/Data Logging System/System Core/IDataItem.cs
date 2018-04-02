/// <summary>
/// Interface for data items.
/// </summary>
public interface IDataItem
{
    Dataset Dataset { get; }
    string ValueAsString { get; }
    void OnDataLoggingRequested();
    void OnEnable();
    void OnDisable();
}