using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthenticationUIManager : MonoBehaviour
{
    public ConnectionManager connManager;
    
    [Header("Connection")]
    public TextMeshProUGUI connStatusTMP;

    [Header("Authentication")]
    public TextMeshProUGUI stateTMP;

    [Header("Log In")]
    public Button login_startbutton;
    public TextMeshProUGUI login_username;
    public TextMeshProUGUI login_password;
    public Button login_button;

    [Header("Sign Up")]
    public Button signup_startbutton;
    public TextMeshProUGUI signup_username;
    public TextMeshProUGUI signup_password;
    public TextMeshProUGUI signup_repeatPassword;
    public Button signup_button;

    [Header("Log Out")]
    public Button logout_button;
    public Button deleteuser_button;

    private void Start()
    {
        if (connManager.initialized)
        {
            LogIn_OnClick();
            SignUp_OnClick();

            logout_button.onClick.AddListener(() =>
            {
                ToggleButtons(false, logout_button);
                connManager.player = new Player();
                connManager.PlayerInitialized();

                SwapLSOD(value: true);
                ToggleButtons(true, logout_button);
                ToggleButtons(true, login_button);
                ToggleButtons(true, signup_button);
            });

            deleteuser_button.onClick.AddListener(() =>
            {
                ToggleButtons(false, deleteuser_button);
                StartCoroutine(connManager.DeleteAccount(connManager.player.id, (res) =>
                {
                    connManager.player = new Player();
                    connManager.PlayerInitialized();

                    SwapLSOD(value: true);
                    ToggleButtons(true, login_button);
                    ToggleButtons(true, signup_button);
                }));
            });

            //Pings to google for internet connection
            StartCoroutine(connManager.CheckConnection((value) =>
            {
                ToggleButtons(value: true, login_button);
                ToggleButtons(value: true, signup_button);
                connStatusTMP.text = value ? "Connected to the internet!" : "Can't connect to the internet!";
            }));
        }
    }

    private void SignUp_OnClick()
    {
        //On SignUp
        signup_button.onClick.AddListener(() =>
        {
            signup_button.interactable = false;
            if (signup_password.text == signup_repeatPassword.text)
            {
                ToggleButtons(value: false, button: signup_button);
                StartCoroutine(connManager.SubmitForm(username: signup_username.text, password: signup_password.text, connManager.signupRoute, (res) =>
                {
                    if (res != null)
                    {
                        stateTMP.text = res[0];
                        ToggleButtons(value: res[0] != "Sign Up Successful", button: login_button);

                        if (res[0] == "Sign Up Successful")
                        {
                            connManager.UpdatePlayer(int.Parse(res[1]), res[2], res[3], int.Parse(res[4]));
                            SwapLSOD(value: false);
                        }
                    }
                }));
            }
            else
            {
                stateTMP.text = "Passwords do not match";
                ToggleButtons(value: true, button: signup_button);
            }
        });
    }

    private void LogIn_OnClick()
    {
        //On Login
        login_button.onClick.AddListener(() =>
        {
            ToggleButtons(value: false, button: login_button);
            StartCoroutine(connManager.SubmitForm(username: login_username.text, password: login_password.text, connManager.loginRoute, (res) =>
            {
                if (res != null)
                {
                    stateTMP.text = res[0];
                    ToggleButtons(value: res[0] != "Login Successful", button: login_button);

                    if (res[0] == "Login Successful")
                    {
                        connManager.UpdatePlayer(int.Parse(res[1]), res[2], res[3], int.Parse(res[4]));
                        SwapLSOD(value: false);
                    }
                }
            }));

        });
    }

    public void SwapLSOD(bool value)
    {
        login_startbutton.gameObject.SetActive(value);
        signup_startbutton.gameObject.SetActive(value);

        logout_button.gameObject.SetActive(!value);
        deleteuser_button.gameObject.SetActive(!value);
    }

    public void ToggleButtons(bool value, Button button)
    {
        button.interactable = value;
    }
}