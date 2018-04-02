using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

/// <summary>
/// Changes graphical profile settings for specific scenes when they are loaded.
/// </summary>
[RequireComponent(typeof(PostProcessVolume))]
public class SceneSpecificGraphics : MonoBehaviour {

    private PostProcessVolume postProcessVolume;
    private AmbientOcclusion ao;

    void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out ao);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Use this for initialization
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {

        // Use the player rendering settings by default
        Player.instance.hmdTransform.GetComponent<Camera>().renderingPath = RenderingPath.UsePlayerSettings;

        switch (scene.name)
        {
            case "Grass Intro":
                ao.active = false;
                break;

            case "Bedroom Scene":
                ao.active = true;
                Player.instance.hmdTransform.GetComponent<Camera>().allowMSAA = false;
                Player.instance.hmdTransform.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                break;

            case "Bedroom Scene Mirrored":
                ao.active = true;
                Player.instance.hmdTransform.GetComponent<Camera>().allowMSAA = false;
                Player.instance.hmdTransform.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                break;

            case "Gym Scene":
                ao.active = false;
                Player.instance.hmdTransform.GetComponent<Camera>().renderingPath = RenderingPath.DeferredShading;
                Player.instance.hmdTransform.GetComponent<Camera>().allowMSAA = false;
                Player.instance.hmdTransform.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                break;

            default:
                Debug.LogWarning("No specific graphics settings defined for this scene.");
                break;
        }

    }


}
