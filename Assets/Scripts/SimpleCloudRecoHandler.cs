using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SimpleCloudRecoHandler : MonoBehaviour
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    
    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    
    public void OnInitialized(TargetFinder targetFinder) {
        Debug.Log ("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError) {
        Debug.Log ("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError) {
        Debug.Log ("Cloud Reco update error " + updateError.ToString());
    }
    
    public void OnStateChanged(bool scanning) 
    {
        Debug.Log("OnStateChanged: scanning=" + scanning);
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }
    }
    
    public ImageTargetBehaviour ImageTargetTemplate;
    [SerializeField] private PaintingInfo paintingInfo;
    
    // Here we handle a cloud target recognition event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) 
    {
        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult = 
            (TargetFinder.CloudRecoSearchResult)targetSearchResult;
        // do something with the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        // stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;
        
        Debug.Log("OnNewSearchResult: TargetName=" + targetSearchResult.TargetName);
        Debug.Log("neta=" + mTargetMetadata);
        
        // Build augmentation based on target 
        if (ImageTargetTemplate) 
        { 
            // enable the new result with the same ImageTargetBehaviour: 
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>(); 
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject); 
        }
        
        var myObject = JsonUtility.FromJson<ObjectInfo>(mTargetMetadata);
        
        if (paintingInfo)
        {
            paintingInfo.InitData(myObject.name, myObject.author, myObject.description);
        }
    }
    
    void OnGUI() {
        // Display current 'scanning' status
        GUI.Box (new Rect(100,100,200,50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box (new Rect(100,200,200,50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning) {
            if (GUI.Button(new Rect(100,300,200,50), "Restart Scanning")) {
                // Restart TargetFinder
                mCloudRecoBehaviour.CloudRecoEnabled = true;
            }
        }
    }
}

[Serializable]
public class ObjectInfo
{
    public string name;
    public string author;
    public string description;
}