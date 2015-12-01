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
	
	void Update () {
        if (!WiimoteManager.HasWiimote())
            return;

        int ret;
        do
        {
            ret = WiimoteManager.Wiimotes[0].ReadWiimoteData();
        } while (ret > 0);
    }
}
