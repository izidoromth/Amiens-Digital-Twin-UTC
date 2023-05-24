using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatabaseConnection;
using DatabaseConnection.Entities;
using DatabaseConnection.Context;
using UnityEngine;

public class DatabaseConnectionTest : MonoBehaviour
{
    public string Verificacao;
    void Start()
    {
        var context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            "postgres", "postgres", "amiens_digital_twin");
        Verificacao = $"{context.Terrains.First(x => x.Name == "amiens").Id}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
