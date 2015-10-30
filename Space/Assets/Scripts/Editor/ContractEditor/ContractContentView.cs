using UnityEngine;
using UnityEditor;

public class ContractContentView : ContractEditorViewBase<ContractContent>
{    
    private Vector2 scrollPos;
    
    [MenuItem("Space/View/Contract/Contract Content")]
    static void Init()
    {
        ContractContentView editor = (ContractContentView)GetWindow(typeof(ContractContentView));
        editor.minSize = new Vector2(600, 600);
        ContractContent.Load();
        editor.InitBase();
        editor.Show();
    }
    
    void OnGUI()
    {
        SetEditorStyles();
    
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
    
        for (int i = 0; i < ContractContent.Data.Count; i++)
            DisplayContractContent(ContractContent.Data[i]);
    
        EditorGUILayout.EndScrollView();
    
        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Data"))
        {
            ContractContent.Load();
        }
    
        GUILayout.FlexibleSpace();
    
        if (GUILayout.Button("New Contract Content"))
        {
            ContractContentEditor newContractEditor = ContractContentEditor.Init();
            newContractEditor.OnClose = ReloadContent;
        }
        GUILayout.Space(6);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
    }
    
    private void DisplayContractContent(ContractContent contractContent)
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        {    
            GUILayout.Label("Tier: " + contractContent.Tier);
            GUILayout.Label("Title: " + contractContent.Title);
            GUILayout.Label("Description: \n" + contractContent.Description);

            ControlsArea(contractContent);
        }
        GUILayout.EndVertical();
    
        GUILayout.Space(12);
    }
    
}
