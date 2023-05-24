using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatabaseConnection;
using DatabaseConnection.Entities;
using DatabaseConnection.Context;
using UnityEngine;
using APIRequest;

public class DatabaseConnectionTest : MonoBehaviour
{
    public bool Verificacao;
    void Start()
    {
        IRepository repo = new APIRequestRepository();
        IRepository repo2 = new LocalRepository();
        Verificacao = repo.GetBuildings().Count == repo2.GetBuildings().Count;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
