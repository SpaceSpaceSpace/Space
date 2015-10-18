using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttachmentToggle : MonoBehaviour
{
	public GameObject Attachment;

	private Toggle m_toggle;

	void Start()
	{
		m_toggle = GetComponent<Toggle>();
	}

	void Update()
	{
		ColorBlock toggleColors = m_toggle.colors;

		if(m_toggle.isOn)
			toggleColors.normalColor = Color.white;
		else
			toggleColors.normalColor = Color.grey;

		m_toggle.colors = toggleColors;
	}
}
