using UnityEngine;
using System.Collections;
using System.IO;

public class ExceptionLogger : MonoBehaviour 
{
	private StreamWriter writer;
	public string LogFileName = "ErrorLog.txt";

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(gameObject);

        writer = new StreamWriter(Path.GetFullPath(".") + "/" + LogFileName);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error || type == LogType.Warning)
        {
            writer.WriteLine("Type: " + type.ToString() + " | Logged at: " + System.DateTime.Now.ToString() + " | Log Desc: " + logString + " | Stack Trace: " + stackTrace);
        }
    }

    void OnEnable() 
	{
        Application.logMessageReceived += HandleLog;
	}

	void OnDisable() 
	{
        Application.logMessageReceived -= HandleLog;
	}
    void OnDestroy()
    {
        writer.Close();
    }
}