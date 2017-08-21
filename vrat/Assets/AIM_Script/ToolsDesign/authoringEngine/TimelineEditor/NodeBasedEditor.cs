using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// from http://gram.gs/gramlog/creating-node-based-editor-unity/

public class NodeBasedEditor : EditorWindow
{
	private List<Node> nodes;
	private List<Connection> connections;
    private List<Node> transitions;

	private GUIStyle nodeStyle;
	private GUIStyle selectedNodeStyle;
    private GUIStyle transitionStyle;
    private GUIStyle selectedTransStyle;


	private GUIStyle inPointStyle;
	private GUIStyle outPointStyle;

	private ConnectionPoint selectedInPoint;
	private ConnectionPoint selectedOutPoint;

	private Vector2 offset;
	private Vector2 drag;

    private int idx = 0;
    private int idx_trans = 0;

	[MenuItem("Window/Node Based Editor")]
	private static void OpenWindow()
	{
		NodeBasedEditor window = GetWindow<NodeBasedEditor>();
		window.titleContent = new GUIContent("Node Based Editor");
	}

	private void OnEnable()
	{
		nodeStyle = new GUIStyle();
		nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;        
		nodeStyle.border = new RectOffset(12, 12, 12, 12);
        
		selectedNodeStyle = new GUIStyle();

		selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
		selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        transitionStyle = new GUIStyle();
        transitionStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node5.png") as Texture2D;
        transitionStyle.border = new RectOffset(12, 12, 12, 12);

        selectedTransStyle = new GUIStyle();
        selectedTransStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node5 on.png") as Texture2D;
        selectedTransStyle.border = new RectOffset(12, 12, 12, 12);


		inPointStyle = new GUIStyle();
		inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
		inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
		inPointStyle.border = new RectOffset(4, 4, 12, 12);

		outPointStyle = new GUIStyle();
		outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
		outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
		outPointStyle.border = new RectOffset(4, 4, 12, 12);
	}

	private void OnGUI()
	{
		DrawGrid(20, 0.2f, Color.gray);
		DrawGrid(100, 0.4f, Color.gray);

		DrawNodes();
		DrawConnections();

		DrawConnectionLine(Event.current);

		ProcessNodeEvents(Event.current);
		ProcessEvents(Event.current);

		if (GUI.changed) Repaint();
	}

	private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
	{
		int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
		int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

		Handles.BeginGUI();
		Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

		offset += drag * 0.5f;
		Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

		for (int i = 0; i < widthDivs; i++)
		{
			Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
		}

		for (int j = 0; j < heightDivs; j++)
		{
			Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
		}

		Handles.color = Color.white;
		Handles.EndGUI();
	}

	private void DrawNodes()
	{
		if (nodes != null)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].Draw();
			}
		}

        if (transitions != null)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].Draw();
            }
        }


	}

	private void DrawConnections()
	{
		if (connections != null)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				connections[i].Draw();
			} 
		}
	}

	private void ProcessEvents(Event e)
	{
		drag = Vector2.zero;

		switch (e.type)
		{
		case EventType.MouseDown:
			if (e.button == 0)
			{
				ClearConnectionSelection();
			}

			if (e.button == 1)
			{
				ProcessContextMenu(e.mousePosition);
			}
			break;

		case EventType.MouseDrag:
			if (e.button == 0)
			{
				OnDrag(e.delta);
			}
			break;
		}
	}

	private void ProcessNodeEvents(Event e)
	{
		if (nodes != null)
		{
			for (int i = nodes.Count - 1; i >= 0; i--)
			{
				bool guiChanged = nodes[i].ProcessEvents(e);

				if (guiChanged)
				{
					GUI.changed = true;
				}
			}
		}

        if (transitions != null)
        {
            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                bool guiChanged = transitions[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }


	}

	private void DrawConnectionLine(Event e)
	{
		if (selectedInPoint != null && selectedOutPoint == null)
		{
			Handles.DrawBezier(
				selectedInPoint.rect.center,
				e.mousePosition,
				selectedInPoint.rect.center + Vector2.left * 50f,
				e.mousePosition - Vector2.left * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}

		if (selectedOutPoint != null && selectedInPoint == null)
		{
			Handles.DrawBezier(
				selectedOutPoint.rect.center,
				e.mousePosition,
				selectedOutPoint.rect.center - Vector2.left * 50f,
				e.mousePosition + Vector2.left * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}
	}

	private void ProcessContextMenu(Vector2 mousePosition)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Add event"), false, () => OnClickAddNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add transition"), false, () => OnClickAddTransition(mousePosition));
		genericMenu.ShowAsContext();
	}

	private void OnDrag(Vector2 delta)
	{
		drag = delta;

		if (nodes != null)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].Drag(delta);
			}
		}

        if (transitions != null)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].Drag(delta);
            }
        }


		GUI.changed = true;
	}
    private void OnClickAddTransition(Vector2 mousePosition)
    {
        idx_trans = idx_trans + 1;
        if (transitions == null)
        {
            transitions = new List<Node>();
        }

        transitions.Add(new Node(false,idx_trans, mousePosition, 200, 100, transitionStyle, selectedTransStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnClickAddAsset));

    }
	private void OnClickAddNode(Vector2 mousePosition)
	{
        idx = idx + 1;
		if (nodes == null)
		{
			nodes = new List<Node>();
		}

		nodes.Add(new Node(true,idx,mousePosition, 300, 150, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,OnClickAddAsset));
	}

	private void OnClickInPoint(ConnectionPoint inPoint)
	{
		selectedInPoint = inPoint;

		if (selectedOutPoint != null)
		{
			if (selectedOutPoint.node != selectedInPoint.node)
			{
				CreateConnection();
				ClearConnectionSelection(); 
			}
			else
			{
				ClearConnectionSelection();
			}
		}
	}

	private void OnClickOutPoint(ConnectionPoint outPoint)
	{
		selectedOutPoint = outPoint;

		if (selectedInPoint != null)
		{
			if (selectedOutPoint.node != selectedInPoint.node)
			{
				CreateConnection();
				ClearConnectionSelection();
			}
			else
			{
				ClearConnectionSelection();
			}
		}
	}

    private void OnClickAddAsset(Node node)
    {

    }

	private void OnClickRemoveNode(Node node)
	{
        
		if (connections != null)
		{
			List<Connection> connectionsToRemove = new List<Connection>();

			for (int i = 0; i < connections.Count; i++)
			{
				if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
				{
					connectionsToRemove.Add(connections[i]);
				}
			}

			for (int i = 0; i < connectionsToRemove.Count; i++)
			{
				connections.Remove(connectionsToRemove[i]);
			}

			connectionsToRemove = null;
		}
        if (node.isEvent == true)
        {
            idx = idx - 1;
            nodes.Remove(node);
        }
        else
        {
            idx_trans = idx_trans - 1;
            transitions.Remove(node);
        }
	}

	private void OnClickRemoveConnection(Connection connection)
	{
		connections.Remove(connection);
	}

	private void CreateConnection()
	{
		if (connections == null)
		{
			connections = new List<Connection>();
		}

		connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
	}

	private void ClearConnectionSelection()
	{
		selectedInPoint = null;
		selectedOutPoint = null;
	}
}