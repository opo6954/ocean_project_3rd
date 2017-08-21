using System;
using UnityEditor;
using UnityEngine;

public class Node
{
	public Rect rect;
	public string title;
	public bool isDragged;
	public bool isSelected;

	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;

	public GUIStyle style;
	public GUIStyle defaultNodeStyle;
	public GUIStyle selectedNodeStyle;

	public Action<Node> OnRemoveNode;
    public Action<Node> OnAddAsset;

    public string contents="";
    public int idx = 0;

    
    public int xOffsetInit = 10;
    public int yOffsetInit = 10;
    
    public int xOffsetNext = 70;
    public int xOffsetParamNext = 100;
    public int yOffsetNext = 20;

    public string p1="";
    public string p2 = "";

    public bool isEvent = false;

    GUIStyle tmpStyle;

    

	public Node(bool _isEvent, int _idx, Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, Action<Node>    OnClickAddAsset)
	{
        idx = _idx;
        isEvent = _isEvent;
		rect = new Rect(position.x, position.y, width, height);
        

		style = nodeStyle;
		inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
		outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
		defaultNodeStyle = nodeStyle;
		selectedNodeStyle = selectedStyle;
		OnRemoveNode = OnClickRemoveNode;
        OnAddAsset = OnClickAddAsset;

        tmpStyle = inPointStyle;
	}

	public void Drag(Vector2 delta)
	{
		rect.position += delta;
	}

    public void DrawEvent()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);

        GUIStyle gs = new GUIStyle();
        GUIStyle label = new GUIStyle();
        GUIStyle param = new GUIStyle();


        label.fontSize = 12;
        param.fontSize = 12;
        param.normal.textColor = Color.white;
        gs.fontStyle = FontStyle.BoldAndItalic;
        gs.fontSize = 15;


        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit, rect.width, rect.height), "Event" + idx.ToString(), label);
        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit + yOffsetNext, rect.width, rect.height), "Asset: ", label);
        GUI.Label(new Rect(rect.x + xOffsetNext, rect.y + yOffsetInit + yOffsetNext, rect.width, rect.height), contents, gs);

        Rect r = new Rect(rect.x + 6, rect.y + yOffsetNext * 3, rect.width - 13, 80);





        GUI.Box(r, title, tmpStyle);

        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit + yOffsetNext * 3, rect.width, rect.height), "Parameter: ", param);


        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit + yOffsetNext * 4, rect.width, rect.height), "Description", param);
        p1 = GUI.TextField(new Rect(rect.x + xOffsetParamNext, rect.y + yOffsetInit + yOffsetNext * 4, 100, 20), p1);

        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit + yOffsetNext * 5, rect.width, rect.height), "RoleInfo", param);
        p2 = GUI.TextField(new Rect(rect.x + xOffsetParamNext, rect.y + yOffsetInit + yOffsetNext * 5, 100, 20), p2);
    }

    public void DrawTrans()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);

        GUIStyle gs = new GUIStyle();
        GUIStyle label = new GUIStyle();
        GUIStyle param = new GUIStyle();


        label.fontSize = 12;
        param.fontSize = 12;
        param.normal.textColor = Color.white;
        gs.fontStyle = FontStyle.BoldAndItalic;
        gs.fontSize = 15;


        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit, rect.width, rect.height), "Transition" + idx.ToString(), label);
        GUI.Label(new Rect(rect.x + xOffsetInit, rect.y + yOffsetInit + yOffsetNext, rect.width, rect.height), "Trigger: ", label);
        GUI.Label(new Rect(rect.x + xOffsetNext, rect.y + yOffsetInit + yOffsetNext, rect.width, rect.height), contents, gs);
    }

	public void Draw()
	{
        if (isEvent == true)
            DrawEvent();
        else
            DrawTrans();
	}

    public void writeTxt(string _contents)
    {
        contents = _contents;
    }


	public bool ProcessEvents(Event e)
	{
		switch (e.type)
		{
		case EventType.MouseDown:
			if (e.button == 0)
			{
				if (rect.Contains(e.mousePosition))
				{
					isDragged = true;
					GUI.changed = true;
					isSelected = true;
					style = selectedNodeStyle;
				}
				else
				{
					GUI.changed = true;
					isSelected = false;
					style = defaultNodeStyle;
				}
			}

			if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
			{
                if (isEvent == true)
                    ProcessContextMenu();
                else
                    ProcessContextMenuTrans();
				e.Use();
			}
			break;

		case EventType.MouseUp:
			isDragged = false;
			break;

		case EventType.MouseDrag:
			if (e.button == 0 && isDragged)
			{
				Drag(e.delta);
				e.Use();
				return true;
			}
			break;
		}

		return false;
	}
    private void ProcessContextMenuTrans()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove transition"), false, OnClickRemoveNode);
        genericMenu.AddItem(new GUIContent("Add trigger/rotate asset"), false, OnClickAddAsset, "Rotate asset");
        genericMenu.AddItem(new GUIContent("Add trigger/walk toward"), false, OnClickAddAsset, "Walk toward");
        genericMenu.AddItem(new GUIContent("Add trigger/touch asset"), false, OnClickAddAsset, "Touch asset");

        genericMenu.ShowAsContext();
    }
	private void ProcessContextMenu()
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Remove event"), false, OnClickRemoveNode);
        genericMenu.AddItem(new GUIContent("Add asset/Exinguisher"), false, OnClickAddAsset,"Extinguisher");
        genericMenu.AddItem(new GUIContent("Add asset/Fire Alarm"), false, OnClickAddAsset, "Fire Alarm");
        genericMenu.AddItem(new GUIContent("Add asset/Life boat"), false, OnClickAddAsset, "Life boat");
        
		genericMenu.ShowAsContext();
	}
       

    private void OnClickAddAsset(object param)
    {

        if (OnAddAsset != null)
        {

            string assetName = (string)param;

            writeTxt(assetName);

            OnAddAsset(this);


            /*
             * private void ProcessContextMenu()
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Remove event"), false, OnClickRemoveNode);
        genericMenu.AddItem(new GUIContent("Add asset"), false, OnClickAddAsset);
		genericMenu.ShowAsContext();
	}
             * */

        }
    }

	private void OnClickRemoveNode()
	{
		if (OnRemoveNode != null)
		{
			OnRemoveNode(this);
		}
	}
}