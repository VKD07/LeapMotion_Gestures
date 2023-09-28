using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Drawing;

public class CameraScript : MonoBehaviour
{
    [SerializeField] string screenShotFolderName;
    [SerializeField] float speed = 1f;
    [SerializeField] Transform targetMiddle;
    [SerializeField] Transform targetLeft;
    [SerializeField] Transform targetRight;

    Transform currentTarget;
    // Update is called once per frame
    void Update()
    {
        ScreenCapture();
        CameraLookAt();
    }

    private void CameraLookAt()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentTarget = targetMiddle;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentTarget = targetLeft;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentTarget = targetRight;
        }

        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            toRotation.x = transform.rotation.x;
            toRotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }

    private void ScreenCapture()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            string folderPath = Application.dataPath + $"/{screenShotFolderName}/";

            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
            UnityEngine.ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 4);
            Debug.Log(folderPath + screenshotName);
        }
    }
}
