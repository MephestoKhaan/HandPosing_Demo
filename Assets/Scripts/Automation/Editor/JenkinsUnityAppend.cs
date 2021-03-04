using System.Collections.Generic;
using System.Linq;
using UnityEditor;

static class JenkinsUnityAppend
{
	public static readonly string[] DEFINES_PROD = new string[] {
		"ANALYTICS_PROD",
	};

	private static void BuildAndroidRelease()
	{
		BuildAndroid(
			keystoreName: GetArg("-keystore"),
			keystorepass: GetArg("-keystorepass"),
			keyaliasName: GetArg("-keyalias"),
			keyaliasPass: GetArg("-keyaliaspass"),
			buildNumber: GetArg("-buildnumber"),
			name: GetArg("-apkname"),
			appID: GetArg("-appid"));
	}

	private static void BuildAndroid(string buildNumber,
		string keystoreName,
		string keystorepass,
		string keyaliasName,
		string keyaliasPass, 
		string name,
		string appID)
	{
		PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);

		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        EditorUserBuildSettings.development = false;
		PlayerSettings.Android.useAPKExpansionFiles = false;
		PlayerSettings.applicationIdentifier = appID;

		AddDefineSymbols(BuildTargetGroup.Android, DEFINES_PROD);

		PlayerSettings.Android.useCustomKeystore = true;
		PlayerSettings.Android.keystoreName = keystoreName;
        PlayerSettings.Android.keystorePass = keystorepass;
        PlayerSettings.Android.keyaliasName = keyaliasName;
        PlayerSettings.Android.keyaliasPass = keyaliasPass;
		
        BuildPipeline.BuildPlayer(GetScenePaths(), $"./{name}.apk", BuildTarget.Android, BuildOptions.None);
    }


	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];

		for (int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}

		return scenes;
	}

	private static string GetArg(string name)
	{
		var args = System.Environment.GetCommandLineArgs();
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == name && args.Length > i + 1)
			{
				return args[i + 1];
			}
		}
		return null;
	}

	private static void AddDefineSymbols(BuildTargetGroup targetGroup, string[] extraDefines)
	{
		string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
		List<string> allDefines = definesString.Split(';').ToList();
		allDefines.AddRange(extraDefines.Except(allDefines));
		definesString = string.Join(";", allDefines.ToArray());

		PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, definesString);
	}
}
