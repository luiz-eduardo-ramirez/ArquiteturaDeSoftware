using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Base.Extensions.Utilites
{
    
    public class CombineMeshes : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu("CombineMeshes")]
        private void Combine()
        {
            Vector3 basePosition = gameObject.transform.position;
            Quaternion baseRotation = gameObject.transform.rotation;
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;

            ArrayList materials = new ArrayList();
            ArrayList combineInstanceArrays = new ArrayList();
            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter meshFilter in meshFilters)
            {
                MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();

                if (!meshRenderer ||
                    !meshFilter.sharedMesh ||
                    meshRenderer.sharedMaterials.Length != meshFilter.sharedMesh.subMeshCount)
                {
                    continue;
                }

                for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
                {
                    int materialArrayIndex = Contains(materials, meshRenderer.sharedMaterials[s].name);
                    if (materialArrayIndex == -1)
                    {
                        materials.Add(meshRenderer.sharedMaterials[s]);
                        materialArrayIndex = materials.Count - 1;
                    }
                    combineInstanceArrays.Add(new ArrayList());

                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
                    combineInstance.subMeshIndex = s;
                    combineInstance.mesh = meshFilter.sharedMesh;
                    (combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
                }
            }

            // Get / Create mesh filter & renderer
            MeshFilter meshFilterCombine = gameObject.GetComponent<MeshFilter>();
            if (meshFilterCombine == null)
            {
                meshFilterCombine = gameObject.AddComponent<MeshFilter>();
            }
            MeshRenderer meshRendererCombine = gameObject.GetComponent<MeshRenderer>();
            if (meshRendererCombine == null)
            {
                meshRendererCombine = gameObject.AddComponent<MeshRenderer>();
            }

            // Combine by material index into per-material meshes
            // also, Create CombineInstance array for next step
            Mesh[] meshes = new Mesh[materials.Count];
            CombineInstance[] combineInstances = new CombineInstance[materials.Count];

            for (int m = 0; m < materials.Count; m++)
            {
                CombineInstance[] combineInstanceArray = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
                meshes[m] = new Mesh();
                meshes[m].indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                meshes[m].CombineMeshes(combineInstanceArray, true, true);
                MeshUtility.SetMeshCompression(meshes[m], ModelImporterMeshCompression.High);

                combineInstances[m] = new CombineInstance();
                combineInstances[m].mesh = meshes[m];
                combineInstances[m].subMeshIndex = 0;
            }
            // Combine into one
            meshFilterCombine.sharedMesh = new Mesh();
            meshFilterCombine.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            meshFilterCombine.sharedMesh.CombineMeshes(combineInstances, false, false);
            MeshUtility.SetMeshCompression(meshFilterCombine.sharedMesh, ModelImporterMeshCompression.High);

            // Destroy other meshes
            foreach (Mesh oldMesh in meshes)
            {
                oldMesh.Clear();
                DestroyImmediate(oldMesh);
            }

            // Assign materials
            Material[] materialsArray = materials.ToArray(typeof(Material)) as Material[];
            meshRendererCombine.materials = materialsArray;

            foreach (MeshFilter meshFilter in meshFilters)
            {
                meshFilter.gameObject.SetActive(false);
                //DestroyImmediate(meshFilter.gameObject);
            }

            gameObject.transform.position = basePosition;
            gameObject.transform.rotation = baseRotation;
        }

        private int Contains(ArrayList searchList, string searchName)
        {
            for (int i = 0; i < searchList.Count; i++)
            {
                if (((Material)searchList[i]).name == searchName)
                {
                    return i;
                }
            }
            return -1;
        }
#endif
    }
}
