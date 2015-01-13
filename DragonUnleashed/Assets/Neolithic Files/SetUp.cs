using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SetUp : MonoBehaviour {
    public string LandFileName = "sampleLand";
    public string CityFileName = "sampleCity";
	// Use this for initialization
	void Start () {
        FileStream s = new FileStream(@"Assets\Neolithic Files\" + LandFileName + ".land", FileMode.Open, FileAccess.Read);

        uint width = 1024;
        uint size = width*width*4;

        byte[] buffer = new byte[size];
        s.Read(buffer, 0, buffer.Length);

        float[,] points = new float[width, width];
        int[,] details = new int[width/2,width/2];
        List<TreeInstance> treeList = new List<TreeInstance>();
        float[, ,] maps = new float[width / 2, width / 2, 4];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float pix = buffer[(i * width + j) * 4] / 256.0f;
                points[i, j] = pix*pix;

                if(i%2==0&&j%2==0)details[i/2,j/2] = buffer[(i * width + j)*4+1]/20;

                if (i % 2 == 0 && j % 2 == 0) maps[i / 2, j / 2, 1] = pix;
                if (i % 2 == 0 && j % 2 == 0) maps[i / 2, j / 2, 0] = 1.0f-pix;
                if (i % 2 == 0 && j % 2 == 0 && pix*256.0f < 28.0f)
                {
                    maps[i / 2, j / 2, 0] = 0.0f;
                    maps[i / 2, j / 2, 2] = 1.0f;
                }
                float traffic = buffer[(i * width + j) * 4 + 3] / 256.0f;
                if (traffic > 0.0f)
                {
                    details[i / 2, j / 2] = 0;
                    maps[i / 2, j / 2, 0] -= traffic;
                    maps[i / 2, j / 2, 1] -= traffic;
                    maps[i / 2, j / 2, 2] -= traffic;
                    maps[i / 2, j / 2, 3] = traffic;
                }

                if ((i % 16 == 0 && j % 16 == 0 && buffer[(j * width + i) * 4 + 2] / 256.0f > 0.4f)||
                    (i % 10 == 0 && j % 10 == 0 && buffer[(j * width + i) * 4 + 2] / 256.0f > 0.5f)||
                    (i % 7 == 0 && j % 7 == 0 && buffer[(j * width + i) * 4 + 2] / 256.0f > 0.6f))
                {
                    int xpos = j + Random.Range(-4, 4);
                    int ypos = i + Random.Range(-4, 4);
                    if (xpos > 1023) xpos = 1023;
                    if (xpos < 0) xpos = 0;
                    if (ypos > 1023) ypos = 1023;
                    if (ypos < 0) ypos = 0;
                    float treeHeight = (buffer[(xpos * width + ypos) * 4] / 256.0f);
                    TreeInstance tree = new TreeInstance();
                    tree.heightScale = 0.5f;
                    tree.color = Color.white;
                    tree.lightmapColor = Color.white;
                    tree.widthScale = 0.5f;
                    tree.prototypeIndex = 0;
                    tree.position = new Vector3(ypos / 1024.0f, treeHeight * treeHeight, xpos / 1024.0f);
                    treeList.Add(tree);
                }
            }
        }

        FileStream cityStream = new FileStream(@"Assets\Neolithic Files\" + CityFileName + ".city", FileMode.Open, FileAccess.Read);

        byte[] farmNumberBuffer = new byte[4];
        cityStream.Read(farmNumberBuffer, 0, farmNumberBuffer.Length);
        int numFarms = System.BitConverter.ToInt32(farmNumberBuffer, 0);

        byte[] farmBuffer = new byte[numFarms * 12];
        cityStream.Read(farmBuffer, 0, farmBuffer.Length);

        int[,] farmDetails = new int[width / 2, width / 2];

        for (int i = 0; i < numFarms; i++)
        {
            int x = System.BitConverter.ToInt32(farmBuffer, i * 12);
            int z = System.BitConverter.ToInt32(farmBuffer, i * 12 + 4);
            int yeild = System.BitConverter.ToInt32(farmBuffer, i * 12 + 8);
            yeild = (yeild > 1) ? 30 : 5;
            for (int j = -2; j < 3; j++)
            {
                for (int k = -2; k < 3; k++)
                {
                    if (z / 2 + j > 0 && z / 2 + j < 511 && x / 2 + k > 0 && x / 2 + k < 511) farmDetails[z / 2 + j, x / 2 + k] = yeild;
                }
            }
        }

        TerrainData bob = new TerrainData();
        bob.size = new Vector3(1024.0f/32.0f,40.0f,1024.0f/32.0f);
        bob.heightmapResolution = 1024;
        bob.SetHeights(0, 0, points);

        DetailPrototype grass = new DetailPrototype();
        Texture2D grasstext = Resources.Load<Texture2D>(@"Neolithic Resources/grass");
        grass.prototypeTexture = grasstext;
        grass.maxHeight = 0.5f;
        DetailPrototype wheat = new DetailPrototype();
        Texture2D wheatText = Resources.Load<Texture2D>(@"Neolithic Resources/wheat");
        wheat.prototypeTexture = wheatText;
        wheat.healthyColor = new Color(1.0f, 1.0f, 0.2f);
        DetailPrototype[] detailProts = {grass,wheat};
        bob.detailPrototypes = detailProts;
        bob.SetDetailResolution(512,8);
        bob.SetDetailLayer(0, 0, 0, details);
        bob.SetDetailLayer(0, 0, 1, farmDetails);

        TreePrototype treePro = new TreePrototype();
        treePro.prefab = Resources.Load<GameObject>(@"Neolithic Resources/tree");
        TreePrototype[] treesProto = {treePro};
        bob.treePrototypes = treesProto;
        bob.treeInstances = treeList.ToArray();

        SplatPrototype lowText = new SplatPrototype();
        lowText.texture = Resources.Load<Texture2D>(@"Neolithic Resources/grassTexture");
        SplatPrototype hiText = new SplatPrototype();
        hiText.texture = Resources.Load<Texture2D>(@"Neolithic Resources/mountainTexture");
        SplatPrototype beachText = new SplatPrototype();
        beachText.texture = Resources.Load<Texture2D>(@"Neolithic Resources/beachTexture");
        SplatPrototype roadText = new SplatPrototype();
        roadText.texture = Resources.Load<Texture2D>(@"Neolithic Resources/pathTexture");
        SplatPrototype[] splats = {lowText,hiText,beachText,roadText};
        bob.splatPrototypes = splats;
        bob.SetAlphamaps(0, 0, maps);

        GameObject steve;
        steve = Terrain.CreateTerrainGameObject(bob);

        GameObject city = new GameObject();

        byte[] houseNumberBuffer = new byte[4];
        cityStream.Read(houseNumberBuffer, 0, houseNumberBuffer.Length);
        int numBuildings = System.BitConverter.ToInt32(houseNumberBuffer, 0);

        byte[] buidingBuffer = new byte[numBuildings * 16];
        cityStream.Read(buidingBuffer, 0, buidingBuffer.Length);

        for (int i = 0; i < numBuildings; i++)
        {
            int x = System.BitConverter.ToInt32(buidingBuffer, i * 16);
            int z = System.BitConverter.ToInt32(buidingBuffer, i * 16 + 4);
            float y = bob.GetHeight(x, z);
            int houseSize = System.BitConverter.ToInt32(buidingBuffer, i * 16 + 8);
            int houseLife = System.BitConverter.ToInt32(buidingBuffer, i * 16 + 12);
            GameObject building;
            if (houseLife < 500)
            {
                if (houseSize > 3) building = GameObject.Instantiate(Resources.Load<GameObject>(@"Neolithic Resources/bigHouseRuin"), new Vector3(x, y, z), new Quaternion(0, Random.Range(-45.0f, 45.0f), 0, 1.0f)) as GameObject;
                else building = GameObject.Instantiate(Resources.Load<GameObject>(@"Neolithic Resources/smallHouseRuin"), new Vector3(x, y, z), new Quaternion(0, Random.Range(-45.0f, 45.0f), 0, 1.0f)) as GameObject;
            }
            else
            {
                if (houseSize > 3) building = GameObject.Instantiate(Resources.Load<GameObject>(@"Neolithic Resources/bigHouse"), new Vector3(x, y, z), new Quaternion(0, Random.Range(-45.0f, 45.0f), 0, 1.0f)) as GameObject;
                else building = GameObject.Instantiate(Resources.Load<GameObject>(@"Neolithic Resources/smallHouse"), new Vector3(x, y, z), new Quaternion(0, Random.Range(-45.0f, 45.0f), 0, 1.0f)) as GameObject;
            }
            building.transform.parent = city.transform;
        }
        bob.name = "Landscape";
        UnityEditor.AssetDatabase.CreateAsset(bob, @"Assets/Landscape.asset");
        bob.SetAlphamaps(0, 0, maps);
        UnityEditor.PrefabUtility.CreatePrefab(@"Assets/World.prefab", city);
        UnityEditor.AssetDatabase.SaveAssets();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}