using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class BarcodeReaderSample : MonoBehaviour
{
    [SerializeField]
    private ARSession session;
    [SerializeField]
    private XROrigin sessionOrigin;
    [SerializeField]
    private ARCameraManager cameraManager;

    private bool scanningEnabled;

    private IBarcodeReader reader = new BarcodeReader
    {
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false
        }   
    };

    private Result result;
    [SerializeField]
    private string lastResult;
    private Texture2D texture;

    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2.
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image).
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();

        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        texture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        texture.LoadRawTextureData(buffer);
        texture.Apply();

        // Done with your temporary data, so you can dispose it.
        buffer.Dispose();

        result = reader.Decode(texture.GetPixels32(), texture.width, texture.height);

        if(result != null)
        {
            if (PlantingManager.instance.TryUnlockPlant(result.Text, out string name))
            {
                SceneManager.instance.ToggleAR();
                return;
            }
        }
    }

    public void ToggleScanning()
    {
        scanningEnabled = !scanningEnabled;
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 256, 25), lastResult);
    }
}
