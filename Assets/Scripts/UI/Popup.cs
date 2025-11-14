using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class Popup : MonoBehaviour {

    public static Popup instance;
    private Button okBtn;
    private Button cancelBtn;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI logText;
    private TMP_InputField inputField;

    [HideInInspector] private Toggle saleOrBuy;
    [HideInInspector] private TextMeshProUGUI saleOrBuyText;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;

        okBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "okBtn");
        cancelBtn = GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.gameObject.name == "cancelBtn");
        titleText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(tmg => tmg.gameObject.name == "titleText");
        logText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(tmg => tmg.gameObject.name == "logText");
        saleOrBuyText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(input => input.gameObject.name == "saleOrBuyText");
        inputField = GetComponentsInChildren<TMP_InputField>().FirstOrDefault(input => input.gameObject.name == "inputField");
        saleOrBuy = GetComponentsInChildren<Toggle>().FirstOrDefault(input => input.gameObject.name == "saleOrBuy");
        saleOrBuy.onValueChanged.AddListener((active) =>
        {
            saleOrBuyText.text = active ? "Sale" : "Buy";
        });
        Hide();
        transform.SetAsLastSibling();
    }
    public bool IsSale()
    {
        return saleOrBuy.isOn == true;
    }
    private void OnEnable()
    {
        saleOrBuy.isOn = false;
    }
    
    private void Start() {
       
    }
    private void Show(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string,TextMeshProUGUI> onOk) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        titleText.text = titleString;

        inputField.characterLimit = characterLimit;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = inputString;
        inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
            return ValidateChar(validCharacters, addedChar);
        };

        inputField.Select();
        okBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
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