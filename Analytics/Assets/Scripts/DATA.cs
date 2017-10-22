using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DATA {

    public static List<Session> sessions = new List<Session>();

    public static void AddSession(string gameName, string dateTime, string sessionID, List<string> lookDataKeys, List<LookData> lookDataValues, List<KeyCode> keyDataKeys, List<Keydata> keyDataValues) {
        sessions.Add(new Session(gameName, dateTime, sessionID, lookDataKeys, lookDataValues, keyDataKeys, keyDataValues));
    }
    
}

public class Session {

    public string gameName;
    public string dateTime;
    public string sessionID;

    public LookDataManager lookData;
	public KeyDataManager keyData;

    public Session(string _gameName, string _dateTime, string _sessionID, List<string> lookDataKeys, List<LookData> lookDataValues, List<KeyCode> keyDataKeys, List<Keydata> keyDataValues) {
        gameName = _gameName;
        dateTime = _dateTime;
        sessionID = _sessionID;
       
		lookData = new LookDataManager (lookDataKeys, lookDataValues);
		keyData = new KeyDataManager(keyDataKeys, keyDataValues);
    }
}
