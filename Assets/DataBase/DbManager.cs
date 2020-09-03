using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;


public class DbManager
{
	public static string appDBPath;

	public static DbAccess db;

	static DbManager()
    {
		
	}

	public static void CreateDataBase()
    {
		db = new DbAccess("URI=file:" + appDBPath);
	}



}