using Dummiesman;
using FastObjUnity.Runtime;
using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class ReadPostgres : MonoBehaviour
{
    void Start()
    {
        string conn_str = $"\r\nServer=127.0.0.1;Port=5433;Database=binary_test;User Id=postgres;Password=12345678;\r\n";
        NpgsqlConnection conn = new NpgsqlConnection(conn_str);
        try
        {
            conn.Open();
            Debug.Log("Database connection succeeded.");
        }
        catch
        {
            Debug.Log("Database connection failed.");
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        List<string> dataItems = new List<string>();

        string sql = "SELECT * FROM testezin";

        var time = Time.realtimeSinceStartup;

        NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
        ds.Reset();
        da.Fill(ds);
        dt = ds.Tables[0];

        Debug.Log(Time.realtimeSinceStartup - time);

        conn.Close();

        var i = 0;
        //create stream and load
        for(int j = 0; j < dt.Rows.Count; j++)
        {
            int[] skip = { };
            if (skip.Contains(j)) continue;
            DataRow dr = dt.Rows[j];

            time = Time.realtimeSinceStartup;

            var textStream = new MemoryStream(dr.ItemArray[1] as byte[]);
            var loadedObj = new OBJLoader().Load(textStream);
            loadedObj.name = dr.ItemArray[0] as string;

            Debug.Log(Time.realtimeSinceStartup - time);
        }
    }

    void Update()
    {
        
    }
}
