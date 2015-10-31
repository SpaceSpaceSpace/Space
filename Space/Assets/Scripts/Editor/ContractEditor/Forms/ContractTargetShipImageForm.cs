using UnityEngine;
using System.Collections;

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
}
