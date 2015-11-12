using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponPopulator : MonoBehaviour {

    List<GameObject> WeaponList;
    ToggleGroup WeaponToggleGroup;

    public Sprite ButtonBackground;

    public float ButtonWidth = 100;
    public float ButtonHeight = 50;

	public GameObject scrollContent;
	public GameObject buttonPrefab;


	// Use this for initialization
	void Start () {
        WeaponList = Resources.LoadAll<GameObject>("ShipPrefabs/Weapons").ToList<GameObject>();
        WeaponToggleGroup = GetComponent<ToggleGroup>();

        //CreateWeaponListButtons();
		DisplayInventory ();
	}

	void OnEnable()
	{
		DisplayInventory ();
	}

	void DisplayInventory()
	{
		Button[] buttons = scrollContent.GetComponentsInChildren<Button> ();
		foreach(Button _button in buttons)
		{
			Destroy(_button.gameObject);
		}

		for(int i = 0; i < GameMaster.playerData.playerInventory.Weapons.Count; i++)
		{
			int _i = i;
			GameObject button = Instantiate(buttonPrefab) as GameObject;
			button.name = i.ToString();
			button.transform.SetParent(scrollContent.transform,false);
			button.GetComponentInChildren<Text>().text = GameMaster.playerData.playerInventory.Weapons[i].Name;
			Toggle toggle = button.GetComponent<Toggle>();
			toggle.group = WeaponToggleGroup;
			AttachmentToggle attachment = button.AddComponent<AttachmentToggle>();
			//attachment.Attachment = GameMaster.playerData.playerInventory.Weapons[i];
		}
	}

    void CreateWeaponListButtons() 
    {
        RectTransform panelRectTrans = this.GetComponent<RectTransform>();
        float Top = panelRectTrans.rect.height / 2 - 130;

        //For every weapon, add a button to this game object
        for (int i = 0; i < WeaponList.Count; i++)
        {
            GameObject weapon = WeaponList[i];
            string wepName = weapon.name;

            //Get the texture of the weapon off of its material
            SpriteRenderer wepSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
            Sprite wepSprite = wepSpriteRenderer.sprite;

            //Create button 
            GameObject button = new GameObject();
            button.AddComponent<CanvasRenderer>();

            Image background = button.AddComponent<Image>();
            background.sprite = ButtonBackground;
            background.type = Image.Type.Sliced;
			background.preserveAspect = true;

            Toggle toggle = button.AddComponent<Toggle>();
            toggle.group = WeaponToggleGroup;

            button.name = wepName + "Button";

            //Set Button position and size
            RectTransform buttonRectTrans = button.GetComponent<RectTransform>();

            Vector3 pos = new Vector3();
            pos[0] = 0;
            pos[1] = Top - (i * ButtonHeight * 3);
            pos[2] = panelRectTrans.position.z + 1; // To make sure the buttons are above the background

            buttonRectTrans.sizeDelta = new Vector2(ButtonWidth, ButtonHeight);

			//Each button needs to store which attachment its using
			AttachmentToggle attachment = button.AddComponent<AttachmentToggle>();
			attachment.Attachment = weapon;

            //Create weapon image
            GameObject wepImage = new GameObject();
            Image wepImageComp = wepImage.AddComponent<Image>();
			wepImageComp.preserveAspect = true;
			wepImage.transform.Rotate(new Vector3(0f,0f,90f));
            wepImageComp.sprite = wepSprite;
            wepImage.name = wepName + "ButtonImage";

            RectTransform weaponImageRectTrans = wepImage.GetComponent<RectTransform>();
            weaponImageRectTrans.sizeDelta = new Vector2(ButtonWidth - 10, ButtonHeight - 10);

            buttonRectTrans.SetParent(panelRectTrans);
            buttonRectTrans.position = pos;
            weaponImageRectTrans.SetParent(buttonRectTrans);

            //Have to set position after setting parents
            weaponImageRectTrans.anchoredPosition3D = Vector3.zero;
            buttonRectTrans.anchoredPosition3D = pos;
        }
    }
}
