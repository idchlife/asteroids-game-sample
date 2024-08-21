using AsteroidsCore.Game.Objects;
using AsteroidsCore.Utils.Geometry;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class AsteroidsGameObjectRendererComponent : MonoBehaviour {
  private AsteroidsGameObject obj { get; set; }

  public bool VisualRotationEnabled { get; set; } = true;

  // Start is called before the first frame update
  void Start() {
    obj.Destroyed += (s, e) => {
      Destroy(gameObject);
    };

    UpdatePosition();

  }

  // Update is called once per frame
  void Update() {
    UpdatePosition();
    if (VisualRotationEnabled) UpdateRotation();
  }

  private void UpdatePosition() {
    transform.position = new Vector3(
      obj.Position.X,
      obj.Position.Y,
      transform.position.z
    );
  }

  private void UpdateRotation() {
    var direction = Vec2.DirectionFromRadians(obj.Rotation);

    transform.up = new Vector3(direction.X, direction.Y, 0);
  }

  public void SetAsteroidsGameObject(AsteroidsGameObject obj) {
    this.obj = obj;

    UpdatePosition();
    UpdateRotation();
  } 
}

