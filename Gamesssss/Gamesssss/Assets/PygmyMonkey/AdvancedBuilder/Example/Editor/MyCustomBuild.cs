using UnityEngine;
using UnityEditor;
using PygmyMonkey.AdvancedBuilder;

// This class MUST be in an "Editor" folder!
public class MyCustomBuild : IAdvancedCustomBuild
{
	/// <summary>
	/// Callback method that is called before each build
	/// </summary>
	public void OnPreBuild(Configuration configuration, System.DateTime buildDate)
	{
		// Print the build destination path
		string buildDestinationPath = configuration.getBuildDestinationPath(AdvancedBuilder.Get().getAdvancedSettings(), buildDate, AdvancedBuilder.Get().getProductParameters());
		Debug.Log(buildDestinationPath);

		// You can retrieve multiple variables from the configuration class
		// Release type name: configuration.releaseType.name
		// Platform name: configuration.platformType.ToString()
		// Distribution platform name: configuration.distributionPlatform.name
		// Platform architecture name: configuration.platformArchitecture.name
		// Texture compression name: configuration.textureProperties.name
		Debug.Log("Do stuff before build");
	}

	/// <summary>
	/// Callback method that is called after each build
	/// </summary>
	public void OnPostBuild(Configuration configuration, System.DateTime buildDate)
	{
		Debug.Log("Do stuff after build");
	}

	public void OnEveryBuildDone()
	{
		Debug.Log("All builds are done.");
	}
}
