//https://stackoverflow.com/questions/46733430/convert-mesh-to-stl-obj-fbx-in-runtime
//edited

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjSaver : MonoBehaviour
{
    [SerializeField] GameObject targetToSave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
    }

    private void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "data");
        path = Path.Combine(path, "carmodel" + ".obj");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        Debug.Log(path);

        MeshFilter meshFilter = targetToSave.GetComponent<MeshFilter>();
        ObjExporter.MeshToFile(meshFilter, path);
    }
}
