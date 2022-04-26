﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using B83.Win32;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;

public enum MediaType
{
    Image,
    Video
}

public class LayerManager : MonoBehaviour
{
    public GameObject LayerField;
    public GameObject RenderField;
    
    public GameObject LayerPrefab;
    
    private List<Layer> Layers = new List<Layer>();

    public Layer[] GetLayers { get => Layers.ToArray(); }

    public Layer AddLayer<T>(MediaType type, string name, object media) where T : Layer
    {
        var layerObject = Instantiate(LayerPrefab, LayerField.transform);
        var LayerScreen = Instantiate(new GameObject(), RenderField.transform);

        layerObject.gameObject.name = name;

        var LayerImage = LayerScreen.AddComponent<RawImage>();

        var AddedLayer = layerObject.AddComponent<T>();;

        AddedLayer.LayerImage = LayerImage;
        
        
        AddedLayer.media = media;
        
        Layers.Add(AddedLayer);

        return AddedLayer;
    }

    void OnEnable ()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }
    void OnFiles(List<string> aFiles, POINT aPos)
    {
        string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
                     aFiles.Aggregate((a, b) => a + "\n\t" + b);
        Debug.Log(str);
        
        foreach (var afile in aFiles)
        {
            AddLayer<VideoLayer>(MediaType.Video, afile, afile);
        }
    }
    
}
