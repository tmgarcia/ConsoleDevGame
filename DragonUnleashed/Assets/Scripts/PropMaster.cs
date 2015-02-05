using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PropMaster : MonoBehaviour, ISerializationCallbackReceiver
{
    public Dictionary<string, Representation> props;
    public List<string> keys = new List<string>();
    public List<Representation> values = new List<Representation>();
    public Representation villager;

    [System.Serializable]
    public class Representation
    {
        public Collider collider;
        public MeshRenderer meshRenderer;
        public MeshFilter mesh;
    }

    void Start() 
    {
        
	}
	
	void Update () 
    {
	    
	}

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var kvp in props)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        props = new Dictionary<string, Representation>();
        for (int i = 0; i != Mathf.Min(keys.Count, values.Count); i++)
        {
            props.Add(keys[i], values[i]);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PropMaster))]
public class PropMasterEditor : Editor
{
    Vector2 scrollPos;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var propMaster = target as PropMaster;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Villager");
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;
        propMaster.villager.mesh = (MeshFilter)EditorGUILayout.ObjectField("Mesh", propMaster.villager.mesh, typeof(MeshFilter), true);
        propMaster.villager.collider = (Collider)EditorGUILayout.ObjectField("Collider", propMaster.villager.collider, typeof(Collider), true);
        propMaster.villager.meshRenderer = (MeshRenderer)EditorGUILayout.ObjectField("MeshRenderer", propMaster.villager.meshRenderer, typeof(MeshRenderer), true);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Props");
        if (GUILayout.Button("+", EditorStyles.miniButtonRight))
        {
            propMaster.keys.Add("");
            propMaster.values.Add(new PropMaster.Representation());
        }
        EditorGUILayout.EndHorizontal();

        if(propMaster.keys.Count > 0)
        {
            int indexToRemove = -1;
            EditorGUILayout.BeginVertical();
            //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(200));
            for (int i = 0; i != Mathf.Min(propMaster.keys.Count, propMaster.values.Count); i++)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Prop " + i);
                if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                {
                    indexToRemove = i;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                propMaster.keys[i] = EditorGUILayout.TextField("Name", propMaster.keys[i]);
                propMaster.values[i].mesh = (MeshFilter)EditorGUILayout.ObjectField("Mesh", propMaster.values[i].mesh, typeof(MeshFilter), true);
                propMaster.values[i].collider = (Collider)EditorGUILayout.ObjectField("Collider", propMaster.values[i].collider, typeof(Collider), true);
                propMaster.values[i].meshRenderer = (MeshRenderer)EditorGUILayout.ObjectField("MeshRenderer", propMaster.values[i].meshRenderer, typeof(MeshRenderer), true);
                EditorGUI.indentLevel--;
            }
            if (indexToRemove != -1)
            {
                propMaster.keys.RemoveAt(indexToRemove);
                propMaster.values.RemoveAt(indexToRemove);
            }
            //EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif