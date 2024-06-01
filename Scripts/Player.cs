using Godot;
using System;

public partial class Player : Node3D
{
	private bool leftPressed = false;
	private bool rightPressed = false;
	private bool forwardPressed = false;

	[Export]
	public StringName fireInput;

	[Export]
	public Node3D CameraAnchor { get; set; }
	[Export]
	public Node3D CameraNode { get; set; }
	[Export]
	public float MouseSensitivity { get; set; }

	[Export]
	public int[] Numbers { get; set; }

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction(fireInput))
		{

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
			CameraAnchor.RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * MouseSensitivity));
			CameraNode.RotateX(Mathf.DegToRad(-mouseEvent.Relative.Y * MouseSensitivity));

			Vector3 cameraRot = CameraNode.RotationDegrees;
			cameraRot.X = Mathf.Clamp(cameraRot.X, -70, 70);
			CameraNode.RotationDegrees = cameraRot;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player Ready");
		Input.UseAccumulatedInput = false;
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
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
