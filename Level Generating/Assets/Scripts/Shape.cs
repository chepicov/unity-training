﻿using UnityEngine;

public class Shape : PersistableObject {
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;


    public int ShapeId {
        get {
            return shapeId;
        }
        set {
            if (shapeId == int.MinValue && value != int.MinValue) {
                shapeId = value;
            } else {
                Debug.LogError("Not allowed to change shapeId.");
            }
        }
    }
	public int MaterialId { get; private set; }

	MeshRenderer meshRenderer;
	int shapeId = int.MinValue;
    Color color;


	void Awake () {
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void SetColor (Color color) {
		this.color = color;
		if (sharedPropertyBlock == null) {
			sharedPropertyBlock = new MaterialPropertyBlock();
		}
		sharedPropertyBlock.SetColor(colorPropertyId, color);
		meshRenderer.SetPropertyBlock(sharedPropertyBlock);
	}

    public void SetMaterial (Material material, int materialId) {
		meshRenderer.material = material;
		MaterialId = materialId;
    }

    public override void Save (GameDataWriter writer) {
		base.Save(writer);
		writer.Write(color);
	}

	public override void Load (GameDataReader reader) {
		base.Load(reader);
		SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
	}
}