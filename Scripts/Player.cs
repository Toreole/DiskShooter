using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public partial class Player : Node3D
{
	private bool leftPressed = false;
	private bool rightPressed = false;
	private bool forwardPressed = false;

	[Export]
	public StringName fireInput;
	[Export]
	private StringName aimInput;

	[Export] 
	private AnimationPlayer animator;

	[Export]
	public Node3D CameraAnchor { get; set; }
	[Export]
	public Camera3D CameraNode { get; set; }
	[Export]
	public float MouseSensitivity { get; set; }
	[Export]
	private float aimingMouseSensitivity;

	[Export]
	public int[] Numbers { get; set; }

	private bool isAiming = false;

	public override void _Input(InputEvent @event)
	{
		if(@event.AsText() == "Escape")
		{
			Input.MouseMode = 0;
		}

		if (@event.IsAction(fireInput))
		{
			
			GD.Print("Fire");
			var spaceState = GetWorld3D().DirectSpaceState;

			var size = GetTree().Root.Size;
			var center = new Vector2(size.X * 0.5f, size.Y * 0.5f);
			var from = CameraNode.ProjectRayOrigin(center);
			var to = from + CameraNode.ProjectRayNormal(center) * 1000f;
			var query = PhysicsRayQueryParameters3D.Create(from, to);

			var result = spaceState.IntersectRay(query);
			if (result != null && result.Count > 0)
			{
				var collider = result["collider"];
				if (collider.Obj is PhysicsBody3D body)
				{
					GD.Print(body.GetType().Name);
					GD.Print(body.Name);
					GD.Print(body.GetPath());

					SphereMesh sphere = new();
					MeshInstance3D meshInstance = new MeshInstance3D();
					meshInstance.Mesh = sphere;
					meshInstance.MaterialOverride = new StandardMaterial3D();
					GetTree().Root.AddChild(meshInstance);
					meshInstance.GlobalPosition = (Vector3)result["position"].Obj;
					DeleteNodeAfterDelay(meshInstance);

					if(body is FlyingTarget target)
					{
						target.Destroy();
					}
				}
			}
			GD.Print("--------------");
		}
		else if (@event.IsAction(aimInput))
		{
			if (@event.IsPressed())
			{
				animator.CurrentAnimation = "aiming_pos";
				animator.Play();
				isAiming = true;
			}
			else
			{
				animator.CurrentAnimation = "default_pos";
				animator.Play();
				isAiming = false;
			}
		}
		if(@event.IsAction("move_left"))
		{
			leftPressed = @event.IsPressed();
		}
		if (@event.IsAction("move_right"))
		{
			rightPressed = @event.IsPressed();
		}
		if (@event.IsAction("move_forward"))
		{
			forwardPressed = @event.IsPressed();
		}
		if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
			var sensitivy = isAiming ? aimingMouseSensitivity : MouseSensitivity;
			CameraAnchor.RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * sensitivy));
			CameraNode.RotateX(Mathf.DegToRad(-mouseEvent.Relative.Y * sensitivy));

			Vector3 cameraRot = CameraNode.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -70, 70);
			CameraNode.RotationDegrees = cameraRot;
		}
	}

	private async void DeleteNodeAfterDelay(Node node)
	{
		await Task.Delay(1000);
		node.QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player Ready");
		Input.UseAccumulatedInput = false;
		
		Input.MouseMode = Input.MouseModeEnum.Captured;

		//to avoid snapping on the first aim
		animator.CurrentAnimation = "default_pos";
		animator.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(leftPressed && !rightPressed)
		{
			this.Translate((float)delta * Vector3.Left);
		}
		if (!leftPressed && rightPressed)
		{
			this.Translate((float)delta * Vector3.Right);
		}
		if (forwardPressed)
		{
			Translate(CameraNode.GlobalTransform.Basis.Z * -(float)delta * 3);
		}
		
	}
}
