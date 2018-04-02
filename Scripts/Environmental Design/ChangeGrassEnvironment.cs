using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeGrassEnvironment : NetworkBehaviour {

    public bool SwitchSceneState;

    public Material MorningSkybox;
    public Material SunsetSkybox;

    public GameObject[] GrassTerrainMeshes;
    public GameObject[] CornTerrainMeshes;

    bool grass = false;
    bool corn = true;

    bool currentSceneState;

	// Use this for initialization
	void Start () {
        currentSceneState = grass;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (SwitchSceneState)
        {
            CmdSwitchSceneState(!currentSceneState);
            SwitchSceneState = false;
        }
	}


    [Command]
    void CmdSwitchSceneState(bool state)
    {
        RpcSwitchSceneState(state);
    }

    [ClientRpc]
    void RpcSwitchSceneState(bool state)
    {

        if (state == grass)
        {
            SetActivityState(GrassTerrainMeshes, false);
            SetActivityState(CornTerrainMeshes, true);
            RenderSettings.skybox = SunsetSkybox;
        }
        else if (state == corn)
        {
            SetActivityState(GrassTerrainMeshes, true);
            SetActivityState(CornTerrainMeshes, false);
            RenderSettings.skybox = MorningSkybox;
        }

        DynamicGI.UpdateEnvironment();
        currentSceneState = state;
    }

    void SetActivityState(GameObject[] meshes, bool value)
    {
        foreach(GameObject mesh in meshes)
        {
            mesh.SetActive(value);
        }
    }
}
