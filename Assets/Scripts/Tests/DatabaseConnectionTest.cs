using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseConnectionTest : MonoBehaviour
{
    public string Verificacao;
    void Start()
    {
        DatabaseConnection.Repository repository = new DatabaseConnection.Repository("postgres", "postgres", "amiens_digital_twin");
        Verificacao = repository.FloodSectors.First(x => x.SectorId == "KE10").SectorId;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
