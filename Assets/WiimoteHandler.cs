using UnityEngine;
using WiimoteApi;

public class WiimoteHandler : MonoBehaviour {

    public static Wiimote PrimaryRemote
    {
        get
        {
            if (!WiimoteManager.HasWiimote())
                return null;
            return WiimoteManager.Wiimotes[0];
        }
    }

    private static WiimoteHandler Singleton = null;

    void OnLevelWasLoaded(int level)
    {
        Singleton = null;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Cleanup");
        while (WiimoteManager.Wiimotes.Count > 0)
            WiimoteManager.Cleanup(WiimoteManager.Wiimotes[0]);
    }

    void Start () {
        if(Singleton != null)
        {
            Debug.LogError("Two WiimoteHandlers in scene at the same time!  Destroying extra WiimoteHandler.");
            Destroy(this);
            return;
        }
        WiimoteManager.FindWiimotes();
        if (WiimoteManager.HasWiimote())
        {
            Wiimote remote = WiimoteManager.Wiimotes[0];
            remote.SendPlayerLED(true, false, false, false);
            remote.SetupIRCamera();
        }
	}

    private int led = 0;
	
	void Update () {
        if (!WiimoteManager.HasWiimote())
            return;

        int led_tentative = (int)(Time.time * 2) % 4;
        if(led != led_tentative)
        {
            led = led_tentative;
            WiimoteManager.Wiimotes[0].SendPlayerLED(led == 0, led == 1, led == 2, led == 3);
        }

        int ret;
        do
        {
            ret = WiimoteManager.Wiimotes[0].ReadWiimoteData();
        } while (ret > 0);
    }
}
