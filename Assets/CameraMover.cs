using UnityEngine;

public class CameraMover : MonoBehaviour {

    [Tooltip("The real-world IR tracker separation, in meters")]
    public float TrackerSeparation = 0.2f;
    [Tooltip("The Horizontol Field of View of the Wii Remote, in Degrees (default 33)")]
    public float WiimoteHorizontalFOV = 33f;
    [Tooltip("The offset of the Wiimote relative to the monitor (if the Wiimote is behind the monitor, for example)")]
    public Vector3 WiimoteOffset = Vector3.zero;
    public float WiimoteAngle = 0;

    public Transform CameraToMove;

    private Vector3 desired = Vector3.zero;
	
	void Update () {
        CameraToMove.localPosition += (desired-CameraToMove.localPosition)/2 * Time.deltaTime * 60;
        if (WiimoteHandler.PrimaryRemote == null)
            return;

        // Points, in "camera space"
        float[,] pts = WiimoteHandler.PrimaryRemote.Ir.GetProbableSensorBarIR();
        if (pts[0, 0] == -1 || pts[1, 0] == -1)
            return;
        pts[0, 0] /= 1023f;
        pts[1, 0] /= 1023f;
        pts[0, 1] /= 767f;
        pts[1, 1] /= 767f;
        float dx_cs = pts[0, 0] - pts[1, 0];
        float dy_cs = pts[0, 1] - pts[1, 1];
        float separation_cs = Mathf.Sqrt(dx_cs * dx_cs + dy_cs * dy_cs);
        if (separation_cs < 2f / 1023f) separation_cs = 2f / 1023f;

        float local_fov = WiimoteHorizontalFOV * separation_cs / 2; // Keep in mind, separation_cs is a fraction of total screen area (0 < x < 1)
        float zdist_real = (TrackerSeparation / 2) / Mathf.Tan(local_fov * Mathf.Deg2Rad);

        Vector2 p = new Vector2((pts[0, 0] + pts[1, 0]) / 2, (pts[0, 1] + pts[1, 1]) / 2);
        p -= new Vector2(0.5f, 0.5f);
        p /= separation_cs;
        p *= TrackerSeparation;

        Vector3 final_offset = new Vector3(-p.x, p.y, -zdist_real);
        final_offset = Quaternion.Euler(WiimoteAngle, 0, 0) * final_offset;
        final_offset += WiimoteOffset;
        desired = final_offset;
    }
}
