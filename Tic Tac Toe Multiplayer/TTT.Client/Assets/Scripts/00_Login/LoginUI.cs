using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Login
{
    public class LoginUI : MonoBehaviour
    {
        [SerializeField] private int _maxUsernameLength = 10;
        [SerializeField] private int _maxPasswordLength = 10;

        private Button _loginButton;
        private TMP_Text _loginText;
        private TMP_InputField _usernameInput;
        private TMP_InputField _passwordInput;
        private TMP_Text _passwordError;
        private TMP_Text _usernameError;
        private Transform _loadingUI;
        private TMP_Text _loginError;

        private bool _isConnected = false;

        private string _username = string.Empty;
        private string _password = string.Empty;

        private void Start()
        {
            _loginButton = transform.Find("Login Button").GetComponent<Button>();
            _loginButton.onClick.AddListener(Login);
            _loginText = _loginButton.transform.Find("Text").GetComponent<TMP_Text>();

            _usernameInput = transform.Find("Username Input").GetComponent<TMP_InputField>();
            _usernameInput.onValueChanged.AddListener(UpdateUsername);
            _usernameError = _usernameInput.transform.Find("Error").GetComponent<TMP_Text>();

            _passwordInput = transform.Find("Password Input").GetComponent<TMP_InputField>();
            _passwordInput.onValueChanged.AddListener(UpdatePassword);
            _passwordError = _passwordInput.transform.Find("Error").GetComponent<TMP_Text>();

            _loadingUI = transform.Find("Loading");
            _loginError = transform.Find("Login Error").GetComponent<TMP_Text>();

            OnAuthFailHandler.OnAuthFail += ShowLoginError;

            NetworkClient.Instance.OnServerConnected += SetIsConnected;
        }

        private void OnDestroy()
        {
            NetworkClient.Instance.OnServerConnected -= SetIsConnected;
            OnAuthFailHandler.OnAuthFail -= ShowLoginError;
        }

        private void SetIsConnected()
        {
            _isConnected = true;
        }

        private void UpdatePassword(string value)
        {
            _password = value;
            ValidateAndUpdateUI();
        }

        private void UpdateUsername(string value)
        {
            _username = value;
            ValidateAndUpdateUI();
        }

        private void ValidateAndUpdateUI()
        {
            var usernameRegex = Regex.Match(_username, "^[a-zA-Z0-9]+$");

            bool interactable =
                (!string.IsNullOrWhiteSpace(_username) &&
                !string.IsNullOrWhiteSpace(_password)) &&
                (_username.Length <= _maxUsernameLength &&
                _password.Length <= _maxPasswordLength) &&
                usernameRegex.Success;

            EnableLoginButton(interactable);

            if (_password != null)
            {
                bool passwordTooLong = _password.Length > _maxPasswordLength;
                _passwordError.gameObject.SetActive(passwordTooLong);
            }

            if (_username != null)
            {
                bool usernameIsInvalid = _username.Length > _maxUsernameLength || !usernameRegex.Success;
                _usernameError.gameObject.SetActive(usernameIsInvalid);
            }
        }

        private void EnableLoginButton(bool interactable)
        {
            _loginButton.interactable = interactable;
            Color color = _loginButton.interactable ? Color.white : Color.gray;
            _loginText.color = color;
            ;
        }

        private void Login()
        {
            StopCoroutine(LoginRoutine());
            StartCoroutine(LoginRoutine());
        }

        private IEnumerator LoginRoutine()
        {
            EnableLoginButton(false);
            _loadingUI.gameObject.SetActive(true);

            NetworkClient.Instance.Connect();

            while (_isConnected == false)
            {
                Debug.Log("WAITING!");
                yield return null;
            }

            Debug.Log("Connected to the server");

            var authRequest = new Net_AuthRequest
            {
                Username = _username,
                Password = _password,
            };
            NetworkClient.Instance.SendServer(authRequest);
        }

        private void ShowLoginError(Net_OnAuthFail obj)
        {
            EnableLoginButton(false);
            _loadingUI.gameObject.SetActive(false);
            _loginError.gameObject.SetActive(true);
        }
    }
}
