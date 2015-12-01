using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PerspectiveShifter : MonoBehaviour {

    public float WindowHeight = 1f;
    public float NearClipPlane = 0.1f;
    public float FarClipPlane = 1000;
    public Camera Controller;
	
	// Update is called once per frame
	void Update () {
        if (Controller == null)
            return;

        Rect r = Controller.pixelRect;
        float aspect = r.width / r.height;
        float WindowWidth = aspect * WindowHeight;
        Vector3 pos = Controller.transform.localPosition;

        // Note, the term in parens in these four variables defines the camera-space viewing rectangle - in other words
        // the transformation of the monitor into the virtual space.  We could plug this directly into PerspectiveOffCenter,
        // but we would have to use the monitor as the near clip plane.  We want to have objects "pop" out of the screen,
        // so we transform the rectangle by essentially sliding the clip plane along the view frustum.
        float left   = NearClipPlane * (-pos.x - WindowWidth / 2) / -pos.z;
        float right  = NearClipPlane * (-pos.x + WindowWidth / 2) / -pos.z;
        float top    = NearClipPlane * (-pos.y + WindowHeight / 2) / -pos.z;
        float bottom = NearClipPlane * (-pos.y - WindowHeight / 2) / -pos.z;

        Controller.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, NearClipPlane, FarClipPlane);
	}

    // From http://docs.unity3d.com/ScriptReference/Camera-projectionMatrix.html
    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }

    void OnDrawGizmos()
    {
        if (Controller == null)
            return;

        Rect r = Controller.pixelRect;
        float WindowWidth =  r.width / r.height * WindowHeight;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(WindowWidth, WindowHeight, 0));
        Gizmos.color = Color.white;

        Gizmos.matrix = Matrix4x4.identity;
    }
}
