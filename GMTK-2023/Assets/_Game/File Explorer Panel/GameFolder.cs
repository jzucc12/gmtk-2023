using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameFolder : MonoBehaviour
{
    [SerializeField] private string folderName;
    [SerializeField] private TextMeshProUGUI fileText;
    [SerializeField] private RectTransform filesObject;
    private bool opened = false;
    private Button button;
    public static event Action ChangeFolder;


    private void OnValidate()
    {
        fileText.text = folderName;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
        Open(false);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }
    
    private void Start()
    {
        Open(false);
    }
    
    private void OnClick()
    {
        Open(!opened);
        ChangeFolder?.Invoke();
    }

    private void Open(bool nowOpen)
    {
        opened = nowOpen;
        filesObject.gameObject.SetActive(nowOpen);
    }
}
