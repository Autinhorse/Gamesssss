using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fenderrio.ImageWarp
{

	[CustomEditor( typeof(RawImageWarp) )]
	public class RawImageWarpEditor : UnityEditor.UI.RawImageEditor {

		public override void OnInspectorGUI ()
		{
			// Draw default inspector content
			base.OnInspectorGUI();

			RawImageWarp imageWarp = target as RawImageWarp;
			bool guiChanged = GUI.changed;

			GUILayout.Space (15);

			SerializedProperty memberProperty = serializedObject.FindProperty ("m_numSubdivisions");
			EditorGUILayout.PropertyField (memberProperty);

			if (!guiChanged && GUI.changed)
			{
				if (memberProperty.intValue < 1)
					memberProperty.intValue = 1;
				else if (memberProperty.intValue > imageWarp.MAX_NUM_SUBDIVISIONS)
					memberProperty.intValue = imageWarp.MAX_NUM_SUBDIVISIONS;
			}

			GUILayout.Space (10);


			memberProperty = serializedObject.FindProperty ("m_cornerOffsetTL");
			EditorGUILayout.PropertyField (memberProperty);

			memberProperty = serializedObject.FindProperty ("m_cornerOffsetTR");
			EditorGUILayout.PropertyField (memberProperty);

			memberProperty = serializedObject.FindProperty ("m_cornerOffsetBR");
			EditorGUILayout.PropertyField (memberProperty);

			memberProperty = serializedObject.FindProperty ("m_cornerOffsetBL");
			EditorGUILayout.PropertyField (memberProperty);



			GUILayout.Space (15);

			if (GUILayout.Button ("Reset Verts"))
			{
				SerializedProperty cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetTR");
				cornerOffsetProperty.vector3Value = Vector3.zero;

				cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetBL");
				cornerOffsetProperty.vector3Value = Vector3.zero;

				cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetBR");
				cornerOffsetProperty.vector3Value = Vector3.zero;

				cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetTL");
				cornerOffsetProperty.vector3Value = Vector3.zero;
			}

			if (GUI.changed)
			{
				imageWarp.ForceUpdateGeometry ();

				serializedObject.ApplyModifiedProperties ();
			}
		}

		void OnSceneGUI()
		{
			RawImageWarp imageWarp = target as RawImageWarp;

			Vector3 currentScale = imageWarp.rectTransform.localScale;

			Vector3 cornerOffset = new Vector3 (imageWarp.rectTransform.rect.width / 2f, imageWarp.rectTransform.rect.height / 2f);

			cornerOffset.Scale (currentScale);

			Vector3 newCornerPosition;
			Vector3 cornerPosition;

			SerializedProperty cornerOffsetProperty;

			bool guiChanged = false;

			// top right
			guiChanged = GUI.changed;
			cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetTR");
			cornerPosition = imageWarp.transform.position + (imageWarp.transform.rotation * cornerOffset);
			newCornerPosition = Handles.PositionHandle (cornerPosition + (imageWarp.transform.rotation * Vector3.Scale( cornerOffsetProperty.vector3Value, currentScale )), imageWarp.transform.rotation);
			newCornerPosition = newCornerPosition - cornerPosition;
			if(!guiChanged && GUI.changed)
				cornerOffsetProperty.vector3Value = Quaternion.Inverse( imageWarp.transform.rotation ) * (new Vector3(newCornerPosition.x / currentScale.x, newCornerPosition.y / currentScale.y, newCornerPosition.z / currentScale.z));

			// bottom left
			guiChanged = GUI.changed;
			cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetBL");
			cornerPosition = imageWarp.transform.position - (imageWarp.transform.rotation * cornerOffset);
			newCornerPosition = Handles.PositionHandle (cornerPosition + (imageWarp.transform.rotation * Vector3.Scale( cornerOffsetProperty.vector3Value, currentScale )), imageWarp.transform.rotation);
			newCornerPosition = newCornerPosition - cornerPosition;
			if(!guiChanged && GUI.changed)
				cornerOffsetProperty.vector3Value = Quaternion.Inverse( imageWarp.transform.rotation ) * (new Vector3(newCornerPosition.x / currentScale.x, newCornerPosition.y / currentScale.y, newCornerPosition.z / currentScale.z));

			// bottom right
			guiChanged = GUI.changed;
			cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetBR");
			cornerPosition = imageWarp.transform.position + (imageWarp.transform.rotation * (new Vector3(cornerOffset.x, -cornerOffset.y)));
			newCornerPosition = Handles.PositionHandle (cornerPosition + (imageWarp.transform.rotation * Vector3.Scale( cornerOffsetProperty.vector3Value, currentScale )), imageWarp.transform.rotation);
			newCornerPosition = newCornerPosition - cornerPosition;
			if(!guiChanged && GUI.changed)
				cornerOffsetProperty.vector3Value = Quaternion.Inverse( imageWarp.transform.rotation ) * (new Vector3(newCornerPosition.x / currentScale.x, newCornerPosition.y / currentScale.y, newCornerPosition.z / currentScale.z) );

			// top left
			guiChanged = GUI.changed;
			cornerOffsetProperty = serializedObject.FindProperty ("m_cornerOffsetTL");
			cornerPosition = imageWarp.transform.position + (imageWarp.transform.rotation * (new Vector3(-cornerOffset.x, cornerOffset.y)));
			newCornerPosition = Handles.PositionHandle (cornerPosition + (imageWarp.transform.rotation * Vector3.Scale( cornerOffsetProperty.vector3Value, currentScale )), imageWarp.transform.rotation);
			newCornerPosition = newCornerPosition - cornerPosition;
			if(!guiChanged && GUI.changed)
				cornerOffsetProperty.vector3Value = Quaternion.Inverse( imageWarp.transform.rotation ) * (new Vector3(newCornerPosition.x / currentScale.x, newCornerPosition.y / currentScale.y, newCornerPosition.z / currentScale.z) );

			serializedObject.ApplyModifiedProperties ();
		}
	}
}