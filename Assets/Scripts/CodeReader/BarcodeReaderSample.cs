using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using UnityEngine.AdaptivePerformance.Provider;
using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Rendering;

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
    private string lastResult;

    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        //Check if scanning is enabled
        if(!scanningEnabled)
        {
            return;
        }

        // Acquire an XRCpuImage
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        // Set up our conversion params
        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Convert the entire image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Output at full resolution
            outputDimensions = new Vector2Int(image.width, image.height),

            // Convert to RGBA format
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image)
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // Create a Texture2D to store the converted image
        var texture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);

        // Texture2D allows us write directly to the raw texture data as an optimization
        var rawTextureData = texture.GetRawTextureData<byte>();

        try
        {
            unsafe
            {
                // Synchronously convert to the desired TextureFormat
                image.Convert(
                    conversionParams,
                    new IntPtr(rawTextureData.GetUnsafePtr()),
                    rawTextureData.Length);
            }
        }
        finally
        {
            // Dispose the XRCpuImage after we're finished to prevent any memory leaks
            image.Dispose();
        }

        result = reader.Decode(texture.GetPixels32(), texture.width, texture.height);
        if(result != null)
        {
            lastResult = result.Text;
            Debug.Log(lastResult);
        }

        // Apply the converted pixel data to our texture
        texture.Apply();
    }

    public void ToggleScanning()
    {
        scanningEnabled = !scanningEnabled;
    }
}
