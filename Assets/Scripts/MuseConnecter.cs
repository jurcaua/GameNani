using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MuseConnecter : MonoBehaviour {

    private LibmuseBridge muse;

    [Header("Test UI")]
    public Button startScanButton;
    public Button connectButton;
    public Button disconnectButton;
    public Dropdown museList;
    public Text dataText;
    public Text connectionText;

    public Text debugText;

    // Use this for initialization
    void Start () {

        // assuming just android for now
        muse = new LibmuseBridgeAndroid();

        registerListeners();
        registerAllData();

        scanForMuses();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void scanForMuses() {

        SetDebugText("Clicked to scan!, " + this.name);
        muse.startListening();
    }

    void receiveMuseList(string data) {
        // This method will receive a list of muses delimited by white space.
        Debug.Log("Found list of muses = " + data);

        // Convert string to list of muses and populate the dropdown menu.
        List<string> muses = data.Split(' ').ToList<string>();
        museList.ClearOptions();
        museList.AddOptions(muses);
    }

    void registerListeners() {
        SetDebugText("Sent name: " + this.name);

        muse.registerMuseListener(this.name, "receiveMuseList");
        muse.registerConnectionListener(this.name, "receiveConnectionPackets");
        muse.registerDataListener(this.name, "receiveDataPackets");
        muse.registerArtifactListener(this.name, "receiveArtifactPackets");
    }

    void registerAllData() {
        // This will register for all the available data from muse headband
        // Comment out the ones you don't want
        muse.listenForDataPacket("ACCELEROMETER");
        muse.listenForDataPacket("GYRO");
        muse.listenForDataPacket("EEG");
        muse.listenForDataPacket("QUANTIZATION");
        muse.listenForDataPacket("BATTERY");
        muse.listenForDataPacket("DRL_REF");
        muse.listenForDataPacket("ALPHA_ABSOLUTE");
        muse.listenForDataPacket("BETA_ABSOLUTE");
        muse.listenForDataPacket("DELTA_ABSOLUTE");
        muse.listenForDataPacket("THETA_ABSOLUTE");
        muse.listenForDataPacket("GAMMA_ABSOLUTE");
        muse.listenForDataPacket("ALPHA_RELATIVE");
        muse.listenForDataPacket("BETA_RELATIVE");
        muse.listenForDataPacket("DELTA_RELATIVE");
        muse.listenForDataPacket("THETA_RELATIVE");
        muse.listenForDataPacket("GAMMA_RELATIVE");
        muse.listenForDataPacket("ALPHA_SCORE");
        muse.listenForDataPacket("BETA_SCORE");
        muse.listenForDataPacket("DELTA_SCORE");
        muse.listenForDataPacket("THETA_SCORE");
        muse.listenForDataPacket("GAMMA_SCORE");
        muse.listenForDataPacket("HSI_PRECISION");
        muse.listenForDataPacket("ARTIFACTS");
    }

    void SetDebugText(string message) {

        debugText.text = message;
        Debug.Log(message);
    }
}
