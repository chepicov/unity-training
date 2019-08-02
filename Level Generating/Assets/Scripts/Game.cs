using UnityEngine;
using System.Collections.Generic;


public class Game : PersistableObject {
    const int saveVersion = 1;

	float creationProgress;
	float destructionProgress;
    List<Shape> shapes;
    string savePath;

	public PersistentStorage storage;
	public KeyCode createKey = KeyCode.C;
	public KeyCode saveKey = KeyCode.S;
	public KeyCode newGameKey = KeyCode.N;
	public KeyCode loadKey = KeyCode.L;
    public KeyCode destroyKey = KeyCode.X;
	public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }

	public ShapeFactory shapeFactory;

    void Awake () {
		shapes = new List<Shape>();
	}

	void Update () {
		if (Input.GetKeyDown(createKey)) {
			CreateShape();
		} else if (Input.GetKey(newGameKey)) {
			BeginNewGame();
		} else if (Input.GetKeyDown(saveKey)) {
			storage.Save(this, saveVersion);
		} else if (Input.GetKeyDown(loadKey)) {
			BeginNewGame();
			storage.Load(this);
		} else if (Input.GetKeyDown(destroyKey)) {
			DestroyShape();
		}
        creationProgress += Time.deltaTime * CreationSpeed;
        while (creationProgress >= 1f) {
			creationProgress -= 1f;
			CreateShape();
		}
        destructionProgress += Time.deltaTime * DestructionSpeed;
        while (destructionProgress >= 1f) {
			destructionProgress -= 1f;
			DestroyShape();
		}
	}

	void BeginNewGame () {
		for (int i = 0; i < shapes.Count; i++) {
			Destroy(shapes[i].gameObject);
		}
		shapes.Clear();
    }

    void CreateShape () {
		Shape instance = shapeFactory.GetRandom();
		Transform t = instance.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);   
        instance.SetColor(Random.ColorHSV(
			hueMin: 0f, hueMax: 1f,
			saturationMin: 0.5f, saturationMax: 1f,
			valueMin: 0.25f, valueMax: 1f,
			alphaMin: 1f, alphaMax: 1f
		));
        shapes.Add(instance);
	}

    public override void Save (GameDataWriter writer) {
		writer.Write(shapes.Count);
		for (int i = 0; i < shapes.Count; i++) {
            writer.Write(shapes[i].MaterialId);
			writer.Write(shapes[i].ShapeId);
			shapes[i].Save(writer);
		}
	}

    void DestroyShape () {
		if (shapes.Count > 0) {
			int index = Random.Range(0, shapes.Count);
			Destroy(shapes[index].gameObject);
			int lastIndex = shapes.Count - 1;
			shapes[index] = shapes[lastIndex];
			shapes.RemoveAt(lastIndex);
		}
	}

    public override void Load (GameDataReader reader) {
		int version = reader.Version;
		if (version > saveVersion) {
			Debug.LogError("Unsupported future save version " + version);
			return;
		}
		int count = version <= 0 ? -version : reader.ReadInt();
		for (int i = 0; i < count; i++) {
            int shapeId = 0;
            int materialId = 0;
            if (version > 0) {
                shapeId = reader.ReadInt();
                materialId = reader.ReadInt();
            }
			Shape instance = shapeFactory.Get(shapeId, materialId);
			instance.Load(reader);
			shapes.Add(instance);
		}
	}
}