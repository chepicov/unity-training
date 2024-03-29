﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Game : PersistableObject {
    const int saveVersion = 2;
    
	float creationProgress;
	float destructionProgress;
    List<Shape> shapes;
    string savePath;
    int loadedLevelBuildIndex;
	[SerializeField] ShapeFactory shapeFactory;
    [SerializeField] PersistentStorage storage;
	[SerializeField] KeyCode createKey = KeyCode.C;
	[SerializeField] KeyCode saveKey = KeyCode.S;
	[SerializeField] KeyCode newGameKey = KeyCode.N;
	[SerializeField] KeyCode loadKey = KeyCode.L;
    [SerializeField] KeyCode destroyKey = KeyCode.X;
    [SerializeField] int levelCount;

    

	public SpawnZone SpawnZoneOfLevel { get; set; }
    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }
	public static Game Instance { get; private set; }

	void OnEnable () {
		Instance = this;
	}

    void Start () {
		shapes = new List<Shape>();
		if (Application.isEditor) {
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene loadedScene = SceneManager.GetSceneAt(i);
				if (loadedScene.name.Contains("Level ")) {
					SceneManager.SetActiveScene(loadedScene);
                    loadedLevelBuildIndex = loadedScene.buildIndex;
					return;
				}
			}
        }
        StartCoroutine(LoadLevel(1));
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
		} else {
			for (int i = 1; i <= levelCount; i++) {
				if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
					BeginNewGame();
					StartCoroutine(LoadLevel(i));
					return;
				}
			}
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
			shapeFactory.Reclaim(shapes[i]);
		}
		shapes.Clear();
    }

	IEnumerator LoadLevel (int levelBuildIndex) {
		enabled = false;
		if (loadedLevelBuildIndex > 0) {
			yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
		}
        yield return SceneManager.LoadSceneAsync(
			levelBuildIndex, LoadSceneMode.Additive
		);
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
        loadedLevelBuildIndex = levelBuildIndex;
		enabled = true;
	}

    void CreateShape () {
		Shape instance = shapeFactory.GetRandom();
		Transform t = instance.transform;
		t.localPosition = SpawnZoneOfLevel.SpawnPoint;
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

    void DestroyShape () {
		if (shapes.Count > 0) {
			int index = Random.Range(0, shapes.Count);
			shapeFactory.Reclaim(shapes[index]);
			int lastIndex = shapes.Count - 1;
			shapes[index] = shapes[lastIndex];
			shapes.RemoveAt(lastIndex);
		}
	}

    public override void Save (GameDataWriter writer) {
		writer.Write(shapes.Count);
		writer.Write(loadedLevelBuildIndex);
		for (int i = 0; i < shapes.Count; i++) {
            writer.Write(shapes[i].MaterialId);
			writer.Write(shapes[i].ShapeId);
			shapes[i].Save(writer);
		}
	}

    public override void Load (GameDataReader reader) {
		int version = reader.Version;
		if (version > saveVersion) {
			Debug.LogError("Unsupported future save version " + version);
			return;
		}
		int count = version <= 0 ? -version : reader.ReadInt();
		StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
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