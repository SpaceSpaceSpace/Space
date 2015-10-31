using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using WyrmTale;

public class ContractTargetShipImageForm : ContractFormBase {

    public int Tier = 1;
    public string TargetShipImagePath = "";

    private Texture2D TargetShipImage;

    public static ContractTargetShipImageForm Init()
    {
        ContractTargetShipImageForm editor = (ContractTargetShipImageForm)GetWindow(typeof(ContractTargetShipImageForm));
        editor.minSize = new Vector2(300, 100);
        editor.Show();

        return editor;
    }

    public static ContractTargetShipImageForm Init(ContractTargetShipImage targetShipImage)
    {
        ContractTargetShipImageForm editor = (ContractTargetShipImageForm)GetWindow(typeof(ContractTargetShipImageForm));
        editor.minSize = new Vector2(300, 100);
        editor.Tier = targetShipImage.Tier;
        editor.TargetShipImagePath = targetShipImage.TargetShipImagePath;
        editor.Show();

        return editor;
    }

    void OnGUI()
    {
        SetEditorStyles();

        Tier = EditorGUILayout.IntSlider("Contract Tier", Tier, 1, 10);

        ImagePreviewArea("Target Ship Image", ref TargetShipImagePath, ref TargetShipImage);

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(closeButtonText))
                AddContractTargetShipImage(new ContractTargetShipImage(Tier, TargetShipImagePath));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(6);
    }

    private void AddContractTargetShipImage(ContractTargetShipImage targetShipImage)
    {
        string filepath = ContractElement.ContractElementFilePath;

        JSON elementJSON = ContractUtils.LoadJSONFromFile(filepath);

        //Do a bit of deserialization to see if any conflicting contracts exist
        List<JSON> contractTargetShipImages = elementJSON.ToArray<JSON>("ContractTargetShipImages").ToList();

        ContractTargetShipImage model = new ContractTargetShipImage(Tier, TargetShipImagePath);

        if (replacementIndex >= 0)
        {
            contractTargetShipImages.RemoveAt(replacementIndex);
            contractTargetShipImages.Insert(replacementIndex, model);
        }
        else
        {
            contractTargetShipImages.Add(model);
        }

        elementJSON["ContractTargetShipImages"] = contractTargetShipImages;

        ContractUtils.WriteJSONToFile(filepath, elementJSON);

        Close();
    }
}
