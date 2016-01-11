using UnityEngine;
using System;

public class ConfigGUI : MonoBehaviour {

    public CameraMover Mover;
    public PerspectiveShifter Shifter;

    private bool ConfigActivated = false;

    void Start()
    {
        winheight = Shifter.WindowHeight.ToString();
        trackerseparation = Mover.TrackerSeparation.ToString();
        hfov = Mover.WiimoteHorizontalFOV.ToString();
        offsetx = Mover.WiimoteOffset.x.ToString();
        offsety = Mover.WiimoteOffset.y.ToString();
        offsetz = Mover.WiimoteOffset.z.ToString();
        angle = Mover.WiimoteAngle.ToString();
    }
    
	void Update () {
        if (Input.GetButtonDown("Toggle Config"))
            ConfigActivated = !ConfigActivated;

        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
	}

    private Rect windowRect = new Rect(20, 20, 400, 50);
    void OnGUI() {
        if(ConfigActivated)
            windowRect = GUILayout.Window(0, windowRect, DrawWindow, "Config");
    }

    private string winheight;
    private string trackerseparation;
    private string hfov;
    private string offsetx;
    private string offsety;
    private string offsetz;
    private string angle;

    void DrawWindow(int windowID)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Window Height (m): ");
        winheight = GUILayout.TextField(winheight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Tracker Separation (m): ");
        trackerseparation = GUILayout.TextField(trackerseparation);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Wiimote Horizontal FOV (deg): ");
        hfov = GUILayout.TextField(hfov);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Wiimote Offset (m): X");
        offsetx = GUILayout.TextField(offsetx);
        GUILayout.Label("Y");
        offsety = GUILayout.TextField(offsety);
        GUILayout.Label("Z");
        offsetz = GUILayout.TextField(offsetz);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Wiimote Angle (deg): ");
        angle = GUILayout.TextField(angle);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Apply"))
        {
            Shifter.WindowHeight = float.Parse(winheight);
            Mover.TrackerSeparation = float.Parse(trackerseparation);
            Mover.WiimoteHorizontalFOV = float.Parse(hfov);
            Mover.WiimoteOffset.x = float.Parse(offsetx);
            Mover.WiimoteOffset.y = float.Parse(offsety);
            Mover.WiimoteOffset.z = float.Parse(offsetz);
            Mover.WiimoteAngle = float.Parse(angle);
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
