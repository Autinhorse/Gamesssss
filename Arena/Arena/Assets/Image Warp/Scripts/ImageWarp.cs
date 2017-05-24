using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fenderrio.ImageWarp
{
	
	[AddComponentMenu("UI/Image Warp")]
	public class ImageWarp : Image {

		public readonly int MAX_NUM_SUBDIVISIONS = 30;

		readonly Vector2 c_xUvDiff = new Vector2(1,0);
		readonly Vector2 c_yUvDiff = new Vector2(0,1);


		[SerializeField] private Vector3 m_cornerOffsetTL;
		public Vector3 cornerOffsetTL { get { return m_cornerOffsetTL; } set { if (SetStruct(ref m_cornerOffsetTL, value)) SetVerticesDirty(); } }

		[SerializeField] private Vector3 m_cornerOffsetTR;
		public Vector3 cornerOffsetTR { get { return m_cornerOffsetTR; } set { if (SetStruct(ref m_cornerOffsetTR, value)) SetVerticesDirty(); } }

		[SerializeField] private Vector3 m_cornerOffsetBR;
		public Vector3 cornerOffsetBR { get { return m_cornerOffsetBR; } set { if (SetStruct(ref m_cornerOffsetBR, value)) SetVerticesDirty(); } }

		[SerializeField] private Vector3 m_cornerOffsetBL;
		public Vector3 cornerOffsetBL { get { return m_cornerOffsetBL; } set { if (SetStruct(ref m_cornerOffsetBL, value)) SetVerticesDirty(); } }

		[SerializeField] private int m_numSubdivisions = 10;
		public int numSubdivisions { get { return m_numSubdivisions; } set { if (SetStruct (ref m_numSubdivisions, value)) SetVerticesDirty (); } }

		private List<UIVertex> m_meshVerts;
		private Vector3[] m_positions = null;
		private int[] m_indices = null;
		private Vector2[] m_uvs = null;

		private Vector3 m_cornerPositionBL;
		private Vector3 m_cornerPositionTL;
		private Vector3 m_cornerPositionTR;
		private Vector3 m_cornerPositionBR;

		private int m_vertRowLength;
		private int m_numVertices;
		private int m_vertIndex = 0;
		private int m_quadIndex = 0;
		private Vector3 m_leftPoint, m_rightPoint;

		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
				return false;

			currentValue = newValue;
			return true;
		}

#if UNITY_EDITOR
		[MenuItem ("Tools/Convert UI Image to UI Image Warp", false, 201)]
		static void ConvertToUIImageWarp ()
		{
			if (ConvertImageToUIImageWarp (Selection.activeGameObject))
			{
				Debug.Log (Selection.activeGameObject.name + "'s Image component converted into a UIImageWarp component");
			}
		}

		public static bool ConvertImageToUIImageWarp(GameObject imageObject)
		{
			Image imageComponent = imageObject.GetComponent<Image> ();
			ImageWarp imageWarpComponent = imageObject.GetComponent<ImageWarp> ();

			if(imageWarpComponent != null)
				return false;

			GameObject tempObject = new GameObject("temp");
			imageWarpComponent = tempObject.AddComponent<ImageWarp>();

			ImageWarp.CopyComponent(imageComponent, imageWarpComponent);

			DestroyImmediate (imageComponent);

			ImageWarp newImageWarpComponent = imageObject.AddComponent<ImageWarp> ();

			ImageWarp.CopyComponent (imageWarpComponent, newImageWarpComponent);

			DestroyImmediate (tempObject);

			return true;
		}

		[MenuItem ("Tools/Convert UI Image to UI Image Warp", true)]
		static bool ValidateConvertToUIImageWarp ()
		{
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Image> () != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		static void CopyComponent(Image imageFrom, Image imageTo)
		{
			imageTo.sprite = imageFrom.sprite;
			imageTo.color = imageFrom.color;
			imageTo.material = imageFrom.material;
			imageTo.enabled = imageFrom.enabled;
			imageTo.raycastTarget = imageFrom.raycastTarget;
			imageTo.type = imageFrom.type;
			imageTo.preserveAspect = imageFrom.preserveAspect;
			imageTo.fillCenter = imageFrom.fillCenter;
			imageTo.fillMethod = imageFrom.fillMethod;
			imageTo.fillAmount = imageFrom.fillAmount;
			imageTo.fillClockwise = imageFrom.fillClockwise;
			imageTo.fillOrigin = imageFrom.fillOrigin;
		}
#endif

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			// get default mesh positions
			base.OnPopulateMesh(vh);

			if (type != Type.Simple)
			{
				Debug.LogWarning ("Slice, Tiled and Filled sprite types aren't supported by UIImageWarp. Please set to 'Simple'.");
				return;
			}

			if (m_meshVerts == null)
			{
				m_meshVerts = new List<UIVertex> ();
			}

			// Grab the default calculated mesh vert positions
			vh.GetUIVertexStream (m_meshVerts);

			vh.Clear();

			// Calculate current corner positions
			m_cornerPositionBL = m_meshVerts [0].position + m_cornerOffsetBL;
			m_cornerPositionTL = m_meshVerts [1].position + m_cornerOffsetTL;
			m_cornerPositionTR = m_meshVerts [2].position + m_cornerOffsetTR;
			m_cornerPositionBR = m_meshVerts [4].position + m_cornerOffsetBR;


			if (m_numSubdivisions < 1)
				m_numSubdivisions = 1;

			m_vertRowLength = m_numSubdivisions + 1;
			m_numVertices = m_vertRowLength * m_vertRowLength;


			if(m_positions == null || m_positions.Length != m_numVertices)
				m_positions = new Vector3[m_numVertices];
			if(m_indices == null || m_indices.Length != (m_numSubdivisions * m_numSubdivisions) * 6)
				m_indices = new int[(m_numSubdivisions * m_numSubdivisions) * 6];
			if(m_uvs == null || m_uvs.Length != m_numVertices)
				m_uvs = new Vector2[m_numVertices];
			
			m_vertIndex = 0;
			m_quadIndex = 0;

			// Step through subdivision verts and calculate positions and uvs
			for(int y=0; y < m_vertRowLength; y++)
			{
				m_leftPoint = m_cornerPositionBL + (m_cornerPositionTL - m_cornerPositionBL) * (y / (float) (m_vertRowLength - 1));
				m_rightPoint = m_cornerPositionBR + (m_cornerPositionTR - m_cornerPositionBR) * (y / (float) (m_vertRowLength - 1));

				for (int x = 0; x < m_vertRowLength; x++)
				{
					m_vertIndex = y * m_vertRowLength + x;

					m_positions[m_vertIndex] = m_leftPoint + (x / (float) (m_vertRowLength - 1)) * (m_rightPoint - m_leftPoint);

					m_uvs[m_vertIndex] = Vector2.zero;
					m_uvs[m_vertIndex] += (c_xUvDiff * (x / (float) (m_vertRowLength - 1)));
					m_uvs[m_vertIndex] += (c_yUvDiff * (y / (float) (m_vertRowLength - 1)));

					if(x != m_vertRowLength - 1 && y != m_vertRowLength - 1)
					{
						m_indices[m_quadIndex * 6 + 0] = m_vertIndex;
						m_indices[m_quadIndex * 6 + 1] = m_vertIndex + m_vertRowLength;
						m_indices[m_quadIndex * 6 + 2] = m_vertIndex + m_vertRowLength + 1;

						m_indices[m_quadIndex * 6 + 3] = m_vertIndex;
						m_indices[m_quadIndex * 6 + 4] = m_vertIndex + m_vertRowLength + 1;
						m_indices[m_quadIndex * 6 + 5] = m_vertIndex + 1;

						m_quadIndex++;
					}
				}
			}

			// add the verts and uvs to the VertexHelper
			for (int idx = 0; idx < m_positions.Length; idx++)
			{
				vh.AddVert(m_positions[idx], color, m_uvs[idx]);
			}

			for (int idx = 0; idx < m_indices.Length; idx += 3)
			{
				vh.AddTriangle(m_indices[idx], m_indices[idx + 1], m_indices[idx + 2]);
			}
		}

		public void ForceUpdateGeometry()
		{
			UpdateGeometry ();

#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				UnityEditor.EditorUtility.SetDirty(this);
			}
#endif
		}

		public void ResetMesh()
		{
			m_cornerOffsetBL = Vector3.zero;
			m_cornerOffsetTL = Vector3.zero;
			m_cornerOffsetTR = Vector3.zero;
			m_cornerOffsetBR = Vector3.zero;

			ForceUpdateGeometry ();
		}

		void OnDrawGizmos()
		{
			Transform _transform = transform;

			for (int idx = 0; idx < m_vertRowLength; idx++)
			{
				Gizmos.color = new Color (1, 1, 1, 0.2f);

				Vector3 lineFrom = _transform.position + _transform.rotation * Vector3.Scale( _transform.localScale, m_positions [idx]);
				Vector3 lineTo = _transform.position + _transform.rotation * Vector3.Scale( _transform.localScale, m_positions [(m_positions.Length - m_vertRowLength) + idx]);

				Gizmos.DrawLine (lineFrom, lineTo);

				lineFrom = _transform.position + _transform.rotation * Vector3.Scale( _transform.localScale, m_positions[idx * m_vertRowLength]);
				lineTo = _transform.position + _transform.rotation * Vector3.Scale( _transform.localScale, m_positions[((idx+ 1) * m_vertRowLength) - 1]);

				Gizmos.DrawLine (lineFrom, lineTo);
			}
		}
	}
}