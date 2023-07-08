using TMPro;
using UnityEngine;

public class FilesUI : MonoBehaviour
{
    private GameFile[] fileButtons;
    [SerializeField] private TextMeshProUGUI filesHeader;
    

    private void Awake()
    {
        fileButtons = GetComponentsInChildren<GameFile>();
    }

    private void Start()
    {
        UpdateFolder(null);
    }

    private void OnEnable()
    {
        GameFolder.ChangeFolder += UpdateFolder;
    }

    private void OnDisable()
    {
        GameFolder.ChangeFolder -= UpdateFolder;
    }

    public void UpdateFolder(GameFolder folder)
    {
        foreach(GameFile file in fileButtons)
        {
            file.gameObject.SetActive(false);
        }

        if(folder == null)
        {
            filesHeader.text = "No folder selected";
            return;
        }

        filesHeader.text = $"Files in {folder.GetName()}";
        for(int ii = 0; ii < folder.GetFiles().Count; ii++)
        {
            fileButtons[ii].gameObject.SetActive(true);
            fileButtons[ii].SetFileSO(folder.GetFiles()[ii]);
        }
    }
}
