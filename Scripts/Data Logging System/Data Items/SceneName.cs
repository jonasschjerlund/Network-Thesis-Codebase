using UnityEngine.SceneManagement;

public class SceneName : DataItem<string> {

    public override void OnDataLoggingRequested()
    {
        value = SceneManager.GetActiveScene().name;
    }
}
