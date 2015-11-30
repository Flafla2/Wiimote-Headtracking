using UnityEngine;

public class CameraMover : MonoBehaviour {

    [Tooltip("The real-world IR tracker separation, in meters")]
    public float TrackerSeparation = 0.2f;
    [Tooltip("The Horizontol Field of View of the Wii Remote, in Degrees (default 33)")]
    public float WiimoteHorizontalFOV = 33f;
    [Tooltip("The Z-axis offset of the Wiimote relative to the monitor (if the Wiimote is behind the monitor, for example)")]
    public float WiimoteOffset = 0;

    public Transform CameraToMove;
	
	void Update () {
        if (WiimoteHandler.PrimaryRemote == null)
            return;

        // Points, in "camera space"
        float[,] pts = WiimoteHandler.PrimaryRemote.Ir.GetProbableSensorBarIR();
        float dx_cs = pts[0, 0] - pts[1, 0];
        float dy_cs = pts[0, 1] - pts[1, 1];
        float separation_cs = Mathf.Sqrt(dx_cs * dx_cs + dy_cs * dy_cs);

        float local_fov = WiimoteHorizontalFOV * separation_cs / 2; // Keep in mind, separation_cs is a fraction of total screen area (0 < x < 1)
        float zdist_real = (TrackerSeparation / 2) / Mathf.Tan(local_fov * Mathf.Rad2Deg);

        Vector2 p = new Vector2((pts[0, 0] + pts[1, 0]) / 2, (pts[0, 1] + pts[1, 1]) / 2);
        p -= new Vector2(0.5f, 0.5f);
        p /= separation_cs;
        p *= TrackerSeparation;

        Vector3 final_offset = new Vector3(p.x, p.y, zdist_real-WiimoteOffset);
        CameraToMove.localPosition = -final_offset;
    }
}
