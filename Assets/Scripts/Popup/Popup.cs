using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;


public class Popup : MonoBehaviour {

    private static Popup instance;
    private Button okBtn;
    private Button cancelBtn;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI logText;
    private TMP_InputField inputField;


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        okBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "okBtn");
        cancelBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "cancelBtn");
        titleText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(tmg => tmg.gameObject.name == "titleText");
        logText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(tmg => tmg.gameObject.name == "logText");
        inputField = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(input => input.gameObject.name == "inputField");

        Hide();
        transform.SetAsLastSibling();
    }

    private void Start() {
       
    }
    private void Show(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string,TextMeshProUGUI> onOk) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        titleText.text = titleString;

        inputField.characterLimit = characterLimit;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Input in here >>>";
        inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
            return ValidateChar(validCharacters, addedChar);
        };

        inputField.text = inputString;
        inputField.Select();
        okBtn.onClick.AddListener(() => {
            onOk(inputField.text,logText);
        });

        cancelBtn.onClick.AddListener(() => {
            Hide();
            onCancel();
        });
    }
    public static void Show_Static(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string,TextMeshProUGUI> onOk) {
        instance.Show(titleString, inputString, validCharacters, characterLimit, onCancel, onOk);
    }
    public static void Show(string titleString, int defaultInt, Action onCancel, Action<int> onOk) 
    {
        instance.Show(titleString, defaultInt.ToString(),"0123456789-", 20, onCancel,
        (string inputText, TextMeshProUGUI logText) =>
        {
            if (int.TryParse(inputText, out int _i)) {
                    onOk(_i);
                } else {
                    onOk(defaultInt);
                }
        });
    }
    private char ValidateChar(string validCharacters, char addedChar) {
        if (validCharacters.IndexOf(addedChar) != -1) {
            // Valid
            return addedChar;
        } else {
            // Invalid
            return '\0';
        }
    }
    
    public static void HideStatic() => instance.Hide();
    
    public void Hide() => gameObject.SetActive(false);


}