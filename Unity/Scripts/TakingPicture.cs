using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingPicture : MonoBehaviour
{
    private static TakingPicture instance;

    private Camera SnapCam;
    private bool TakePicOnNextFrame;

    private void Awake()
    {
        instance = this;
        SnapCam = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (TakePicOnNextFrame)
        {
            TakePicOnNextFrame = false;
            RenderTexture renderTexture = SnapCam.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, true);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            byte[] bytearray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraSCShot.png", bytearray);
            Debug.Log("save SC");
            RenderTexture.ReleaseTemporary(renderTexture);
            SnapCam.targetTexture = null;
        }
    }

    private void TakeSC(int width, int height)
    {
        SnapCam.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        TakePicOnNextFrame = true;
    }

    public static void TakeSC_Static(int width, int height)
    {
        width = 84;
        height = 87;
        instance.TakeSC(width, height);
    }


}
