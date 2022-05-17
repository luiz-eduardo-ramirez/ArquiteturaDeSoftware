using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Base.Extensions.Attributes;

namespace Base.Extensions.Utilites
{
	public class PrefabLightmapData : MonoBehaviour
	{
		[System.Serializable]
		struct RendererInfo
		{
			public Renderer renderer;
			public int LightmapIndex;
			public Vector4 LightmapOffsetScale;
		}

		[SerializeField, ReadOnly]
		RendererInfo[] m_rendererInfo;
		[SerializeField, ReadOnly]
		Texture2D[] m_lightMaps;
		[SerializeField, ReadOnly]
		Texture2D[] m_dirLightmaps;

		[SerializeField]
		bool m_applied = false;

		void Awake()
		{
			if (m_rendererInfo == null || m_rendererInfo.Length == 0)
				return;

			ApplyLightmaps();
		}

		void ApplyLightmaps()
		{
			if (m_applied)
				return;

			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			LightmapData[] combinedLightmaps = new LightmapData[lightmaps.Length + m_lightMaps.Length];

			lightmaps.CopyTo(combinedLightmaps, 0);
			for (int i = 0; i < m_lightMaps.Length; i++)
			{
				combinedLightmaps[i + lightmaps.Length] = new LightmapData();
				combinedLightmaps[i + lightmaps.Length].lightmapColor = m_lightMaps[i];
				combinedLightmaps[i + lightmaps.Length].lightmapDir = m_dirLightmaps[i];
			}

			ApplyRendererInfo(m_rendererInfo, lightmaps.Length);
			LightmapSettings.lightmaps = combinedLightmaps;

			m_applied = true;
		}

		static void ApplyRendererInfo(RendererInfo[] infos, int lightmapOffsetIndex)
		{
			for (int i = 0; i < infos.Length; i++)
			{
				var info = infos[i];

#if UNITY_EDITOR
				if (UnityEditor.GameObjectUtility.AreStaticEditorFlagsSet(info.renderer.gameObject, UnityEditor.StaticEditorFlags.BatchingStatic))
				{
					Debug.LogWarning("The renderer " + info.renderer.name + " is marked Batching Static. The static batch is created when building the player. " +
									 "Setting the lightmap scale and offset will not affect lightmapping UVs as they have the scale and offset already burnt in.", info.renderer);
				}
#endif

				info.renderer.lightmapIndex = info.LightmapIndex + lightmapOffsetIndex;
				info.renderer.lightmapScaleOffset = info.LightmapOffsetScale;
			}
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Assets/Bake Prefab Lightmaps")]
		static void GenerateLightmapInfo()
		{
			if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
			{
				Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
				return;
			}
			UnityEditor.Lightmapping.Bake();

			PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();

			foreach (PrefabLightmapData instance in prefabs)
			{
				GameObject gameObject = instance.gameObject;
				List<RendererInfo> rendererInfos = new List<RendererInfo>();
				List<Texture2D> lightmaps = new List<Texture2D>();
				List<Texture2D> dirLightMaps = new List<Texture2D>();

				GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, dirLightMaps);

				instance.m_rendererInfo = rendererInfos.ToArray();
				instance.m_lightMaps = lightmaps.ToArray();
				instance.m_dirLightmaps = dirLightMaps.ToArray();

				GameObject targetPrefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;
				if (targetPrefab != null)
				{
					//UnityEditor.Prefab
					UnityEditor.PrefabUtility.SaveAsPrefabAsset(gameObject, UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject));
				}
			}
		}

		static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> lightmaps, List<Texture2D> lightmapsDir)
		{
			var renderers = root.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
			{
				if (renderer.lightmapIndex != -1)
				{
					RendererInfo info = new RendererInfo();
					info.renderer = renderer;
					info.LightmapOffsetScale = renderer.lightmapScaleOffset;

					Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
					Texture2D dirLightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapDir;

					info.LightmapIndex = lightmaps.IndexOf(lightmap);
					if (info.LightmapIndex == -1)
					{
						info.LightmapIndex = lightmaps.Count;
						lightmaps.Add(lightmap);
						lightmapsDir.Add(dirLightmap);
					}

					rendererInfos.Add(info);
				}
			}
		}

		[UnityEditor.MenuItem("Assets/Apply Lightmapped Prefabs")]
		static void RefreshLightmapInfo()
		{
			PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();

			foreach (var instance in prefabs)
			{
				instance.ApplyLightmaps();
			}
		}
#endif

	}
}