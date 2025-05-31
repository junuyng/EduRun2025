using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignUp : UIBase
{
    [SerializeField] private bool isTest = false;
    [SerializeField] private TMP_InputField idInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button signUpButton;

    private void Start()
    {
        signUpButton.onClick.AddListener(OnSignUpClicked);
    }

    private void OnSignUpClicked()
    {
        if (isTest)
        {
            return;
        }

        string id = idInput.text;
        string pw = passwordInput.text;

        StartCoroutine(LoginManager.Instance.SignUpRequest(id, pw, (success, message) =>
        {
            if (success)
            {
                UIManager.Instance.Hide<UISignUp>();
                UIManager.Instance.Show<UILobby>();
                UIManager.Instance.Show<UILoading>().StartFakeOnlyLoading(2f);
            }
            else
            {
            }
        }));
    }
}