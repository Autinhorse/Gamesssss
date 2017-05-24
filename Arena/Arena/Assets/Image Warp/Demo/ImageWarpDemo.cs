using UnityEngine;
using Fenderrio.ImageWarp;

public class ImageWarpDemo : MonoBehaviour {

	public ImageWarp m_imageWarper;

	void Start ()
	{
		m_imageWarper.cornerOffsetBL = Vector3.zero;
		m_imageWarper.cornerOffsetTL = new Vector3(-20f, 20f, 0);
		m_imageWarper.cornerOffsetTR = new Vector3(20f, 20f, 0);
		m_imageWarper.cornerOffsetBR = Vector3.zero;

		m_imageWarper.numSubdivisions = 12;
	}
}