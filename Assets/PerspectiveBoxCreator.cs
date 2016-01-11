using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PerspectiveBoxCreator : MonoBehaviour {
    public Transform Left;
    public Transform Right;
    public Transform Up;
    public Transform Down;
    public Transform Back;

    public PerspectiveShifter Shifter;
    public float Depth;

	void Update () {
        if (!Shifter || !Left || !Right || !Up || !Down || !Back || Depth <= 0)
            return;

        Rect r = Shifter.Controller.pixelRect;
        float WindowWidth = r.width / r.height * Shifter.WindowHeight;
        float WindowHeight = Shifter.WindowHeight;

        Left.parent = Shifter.transform;
        Right.parent = Shifter.transform;
        Up.parent = Shifter.transform;
        Down.parent = Shifter.transform;
        Back.parent = Shifter.transform;

        Left.localPosition  = new Vector3(-WindowWidth / 2, 0, Depth / 2);
        Right.localPosition = new Vector3(WindowWidth / 2, 0, Depth / 2);
        Down.localPosition  = new Vector3(0, -WindowHeight / 2, Depth / 2);
        Up.localPosition    = new Vector3(0, WindowHeight / 2, Depth / 2);
        Back.localPosition  = new Vector3(0, 0, Depth);

        Left.localRotation  = Quaternion.LookRotation(Vector3.left, Vector3.forward);
        Right.localRotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);
        Up.localRotation    = Quaternion.LookRotation(Vector3.up, Vector3.forward);
        Down.localRotation  = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        Back.localRotation  = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        Left.localScale = new Vector3(WindowHeight, Depth + 0.1f, 1);
        Right.localScale= new Vector3(WindowHeight, Depth + 0.1f, 1);
        Up.localScale   = new Vector3(WindowWidth, Depth + 0.1f, 1);
        Down.localScale = new Vector3(WindowWidth, Depth + 0.1f, 1);
        Back.localScale = new Vector3(WindowWidth, WindowHeight, 1);
    }
}
