﻿using System;
using System.Collections;
using System.Collections.Generic;
using AVJ.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : InterectableUI, IUIInitializer
{
    public Layer layer;

    public LayoutElement LayoutController;

    public Text Title;
    public RawImage line;

    public RawImage View;

    public Vector3 lastPos;
    

    public void Initialize()
    {
        Title.text = $"[{layer.Type.ToString()}] {layer.gameObject.name}";
        InitTimeline();
        LayoutController = SetComponent<LayoutElement>();
        
        if (layer.Type == LayerType.Image && UIObject)
        {
            View.texture = (Texture2D)layer.media;
        }
        
        IsReady = true;
    }
    
    public void InitTimeline()
    {
        // Outline Component Vailed Check
        if (!UIObject) UIObject = SetComponent<RawImage>();
        if (!collider) collider = SetComponent<BoxCollider2D>();
        
        Debug.Log($"[Timeline, {gameObject.name}] Initializing Timeline");
    }

    public override void OnUIDrag(IDragDropHandler UIConponent)
    {

        LayoutController.ignoreLayout = true;
        var blank = rectTransform.parent.Find("Blank");
        blank.SetSiblingIndex(rectTransform.GetSiblingIndex());
        blank.GetComponent<LayoutElement>().ignoreLayout = false;
        rectTransform.SetSiblingIndex(rectTransform.parent.childCount);
        
        lastPos = rectTransform.localPosition;
    }

    public override void OnUIDrop(IDragDropHandler UIConponent)
    {
        
        var blank = rectTransform.parent.Find("Blank");
        rectTransform.SetSiblingIndex(blank.GetSiblingIndex());
        blank.SetSiblingIndex(rectTransform.parent.childCount);
        blank.GetComponent<LayoutElement>().ignoreLayout = true;
        LayoutController.ignoreLayout = false;
        
        layer.rectTransform.SetSiblingIndex(rectTransform.GetSiblingIndex());
        layer.UIObject.rectTransform.SetSiblingIndex(rectTransform.GetSiblingIndex());
    }

    private void OnMouseDrag()
    {
        if (IsSelected)
        {
            var sib = CalTargetSiblingIndex();
            Debug.Log(sib);
            var blank = rectTransform.parent.Find("Blank"); 
            blank.SetSiblingIndex(sib);
        }
    }

    public int CalTargetSiblingIndex()
    {
        var start = rectTransform.parent.GetChild(0).localPosition.y;
        for (int i = 0; i < rectTransform.parent.childCount; i++)
        {
            Debug.Log(start);
            if (start <= rectTransform.localPosition.y) return i;
            start += ((RectTransform) rectTransform.parent.GetChild(i)).rect.y * 2;
        }
        
        Debug.Log("End");
        
            

        return rectTransform.parent.childCount;
    }

    public void DeleteLayer()
    {
        var layerEvent = new LayerEvent();
        layerEvent.EventType = LayerEventType.Delete;
        layerEvent.layer = layer;
        
        EventManager.Events.Enqueue(layerEvent);
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(layer) if (layer.Type == LayerType.Video && UIObject)
        {
            View.texture = ((RawImage)((VideoLayer)layer).UIObject).texture;
        }
    }
}
