using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PygmyMonkey.AdvancedBuilder
{
	[Serializable]
	public class AdvancedBuilder : ScriptableObject
	{
		/*
		 * Product parameters
		 */
		[SerializeField] private ProductParameters m_productParameters = new ProductParameters();

		public ProductParameters getProductParameters()
		{
			return m_productParameters;
		}


		/*
		 * Release types
		 */
		[SerializeField] private ReleaseTypes m_releaseTypes = new ReleaseTypes();

		public ReleaseTypes getReleaseTypes()
		{
			return m_releaseTypes;
		}

		
		/*
		 * Suppoted platforms
		 */
		[SerializeField] private Platforms m_platforms = new Platforms();

		public Platforms getPlatforms()
		{
			return m_platforms;
		}
		
		
		/*
		 * Advanced settings
		 */
		[SerializeField] private AdvancedSettings m_advancedSettings = new AdvancedSettings();
		
		public AdvancedSettings getAdvancedSettings()
		{
			return m_advancedSettings;
		}
		
		
		/*
		 * Advanced settings
		 */
		[SerializeField] private ProjectConfigurations m_projectConfigurations = new ProjectConfigurations();
		
		public ProjectConfigurations getProjectConfigurations()
		{
			return m_projectConfigurations;
		}


		public static void PerformBuild()
		{
			PerformBuild(null);
		}


		public static void PerformBuild(Configuration configuration)
		{
			AdvancedBuilder advancedBuilder = Get();

			AppParametersHelper.SaveBuildTarget();

			DateTime buildDate = DateTime.Now;

			List<Configuration> configurationList = advancedBuilder.getProjectConfigurations().configurationList.Where(x => x.isEnabled).ToList();
			if (configuration != null)
			{
				configurationList = new List<Configuration>() { configuration };
			}

			foreach (Configuration config in configurationList)
			{
				PlatformBuilder platformBuilder = new PlatformBuilder(advancedBuilder, buildDate);
				platformBuilder.performBuild(config);
			}

			AppParametersHelper.RestoreBuildTarget();

			if (advancedBuilder.getAdvancedSettings().customBuildMonoScript != null)
			{
				IAdvancedCustomBuild customBuild = (IAdvancedCustomBuild)System.Activator.CreateInstance(advancedBuilder.getAdvancedSettings().customBuildMonoScript.GetClass());
				customBuild.OnEveryBuildDone();
			}
		}

		private static string m_PackageFilePath;
		public static string GetPackagePath()
		{
			if (string.IsNullOrEmpty(m_PackageFilePath) || !File.Exists(Application.dataPath + "/" + m_PackageFilePath + "AdvancedBuilder/Editor/AdvancedBuilderWindow.cs"))
			{
				string[] filePathArray = Directory.GetFiles(Application.dataPath, "AdvancedBuilderWindow.cs", SearchOption.AllDirectories);

				if (filePathArray.Length == 0)
				{
					throw new UnityException("Error finding the Advanced Builder folder!");
				}
				else if (filePathArray.Length > 1)
				{
					throw new UnityException("Found more than one AdvancedBuilderWindow.cs file... Seems like something's wrong with the Advanced Builder integration...");
				}

				m_PackageFilePath = filePathArray[0];
				m_PackageFilePath = m_PackageFilePath.Replace("\\", "/").Replace("AdvancedBuilder/Editor/AdvancedBuilderWindow.cs", string.Empty).Replace(Application.dataPath, "Assets");
			}

			return m_PackageFilePath;
		}

		private static AdvancedBuilder m_Instance;
		public static AdvancedBuilder Get()
		{
			if (m_Instance == null)
			{
				m_Instance = (AdvancedBuilder)AssetDatabase.LoadAssetAtPath(GetPackagePath() + "AdvancedBuilder/Editor/AdvancedBuilder.asset", typeof(AdvancedBuilder));
			}

			return m_Instance;
		}
	}
}
