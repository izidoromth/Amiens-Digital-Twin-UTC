using Assets.Scripts;
using DatabaseConnection.Context;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    public TMP_InputField Hostname;
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField Port;
    public TMP_InputField Database;
    public Button Connect;

    void Start()
    {
        Hostname.text = PlayerPrefs.GetString("hostname");
        Username.text = PlayerPrefs.GetString("username");
        Password.text = PlayerPrefs.GetString("password");
        Port.text = PlayerPrefs.GetString("port");
        Database.text = PlayerPrefs.GetString("database");

        Connect.onClick.AddListener(delegate ()
        {
            ConnectionHelper.Context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            Username.text, Password.text, Database.text, Hostname.text, Port.text);

            if (ConnectionHelper.Context.Database.CanConnect())
            {
                PlayerPrefs.SetString("hostname", Hostname.text);
                PlayerPrefs.SetString("username", Username.text);
                PlayerPrefs.SetString("password", Password.text);
                PlayerPrefs.SetString("port", Port.text);
                PlayerPrefs.SetString("database", Database.text);
                PlayerPrefs.Save();
                SceneManager.LoadScene("DigitalTwin");
            }
        });
    }
}
