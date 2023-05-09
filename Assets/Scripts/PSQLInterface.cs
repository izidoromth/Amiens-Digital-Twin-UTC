using Dummiesman;
using FastObjUnity.Runtime;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PSQLInterface
{
    static NpgsqlConnection dbConnection;

    public static void PSQLConnect(string host, string port, string db_name, string username, string password)
    {
        string conn_str = $"\r\nServer={host};Port={port};Database={db_name};User Id={username};Password={password};\r\n"; 
        NpgsqlConnection conn = new NpgsqlConnection(conn_str);
        try
        {
            conn.Open();
            Debug.Log("Database connection succeeded.");
            dbConnection = conn;
        }
        catch
        {
            Debug.Log("Database connection failed.");
            dbConnection = null;
        }
    }

    public static DataTable PGSQLExecuteSelectQuery(string table, string columns = "*", string condition = "")
    {
        if (dbConnection == null)
            throw new System.Exception("PGSQL Connection is null");

        DataSet ds = new DataSet();
        List<string> dataItems = new List<string>();

        string sql = string.IsNullOrEmpty(condition) ? $"SELECT {columns} FROM {table}": $"SELECT {columns} FROM {table} WHERE condition";

        NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, dbConnection);
        ds.Reset();
        da.Fill(ds);

        return ds.Tables[0];
    }

    public static void PGSQLCloseConnection()
    {
        dbConnection.Close();
    }

    void LoadGameObjectFromOBJ()
    {
        //var mat = Resources.Load<Material>("Sectors");
        //var textStream = new MemoryStream(dr.ItemArray[1] as byte[]);
        //var loadedObj = new OBJLoader().Load(textStream);
        //loadedObj.name = dr.ItemArray[0] as string;
        //var meshRenderer = loadedObj.GetComponentsInChildren<MeshRenderer>()[0];
        //meshRenderer.materials = new Material[] { mat };
    }
}
