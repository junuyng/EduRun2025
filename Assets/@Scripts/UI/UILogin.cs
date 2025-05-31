 using TMPro;
using UnityEngine;
 using UnityEngine.UI;
 
public class UILogin : UIBase
{    
    [SerializeField] private bool isTest = false;
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signUpButton;

    private AudioUnit unit;
    
    
    private void Start()
    {
        unit = GetComponent<AudioUnit>();
        loginButton.onClick.AddListener(OnLoginClicked);
        signUpButton.onClick.AddListener(OnSignUpClicked);
    }

    private void OnLoginClicked()
    {
        
        if (isTest)
        {
            UIManager.Instance.Show<UILobby>();
            UIManager.Instance.Hide<UILogin>();
            return;
        }
        
        string id = idInput.text;
        string pw = passwordInput.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            return;
        }
        
        
        StartCoroutine(LoginManager.Instance.LoginRequest(id, pw, (success, message) =>
        {
            if (success)
            {
            }
            else
            {
            }
        }));
    }

   private void OnSignUpClicked()
   {
       UIManager.Instance.Show<UISignUp>();
       UIManager.Instance.Hide<UILogin>();
   }
}
