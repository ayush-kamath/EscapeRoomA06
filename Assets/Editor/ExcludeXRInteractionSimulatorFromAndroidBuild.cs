using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

// The simulator creates a virtual HMD and removes real HMD input devices by default.
// Keep it in the scene for Editor play mode, but omit it from Quest builds.
[BuildCallbackVersion(1)]
internal sealed class ExcludeXRInteractionSimulatorFromAndroidBuild : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        if (!BuildPipeline.isBuildingPlayer || report == null || report.summary.platform != BuildTarget.Android)
            return;

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root.GetComponent<XRInteractionSimulator>() == null)
                continue;

            Debug.Log($"Excluding XR Interaction Simulator from Android build scene: {scene.path}");
            Object.DestroyImmediate(root);
        }
    }
}
