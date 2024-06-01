using Godot;
using System;

public partial class FlyingTarget : RigidBody3D
{
	[Export(PropertyHint.NodeType, "GpuParticles3D")]
	public PackedScene DestroyParticles { get; set; }
	[Export]
	public float LeftRightDrift { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		var velocity = LinearVelocity;
		if ( !Mathf.IsZeroApprox(LeftRightDrift))
		{
			velocity = velocity.Rotated(Vector3.Up, Mathf.DegToRad((float)delta * LeftRightDrift));
			this.LinearVelocity = velocity;
		}

		this.GravityScale = Mathf.Max(0, 
			Mathf.Min(1 / ((1- Mathf.Abs(velocity.Normalized().Y)) * Mathf.Log(velocity.LengthSquared())), 1)
			);

		//float horizontalAngle = 0;
		//var horizontal = velocity; horizontal.Y = 0;
		//horizontalAngle = horizontal.AngleTo(horizontal);

		this.LookAt(GlobalPosition + velocity, Vector3.Up);

	}

	public void Destroy()
	{
		GD.Print("Destroy");
		var node = DestroyParticles.Instantiate<GpuParticles3D>();
		node.Emitting = true;
		this.GetParent().AddChild(node);
		node.GlobalPosition = this.GlobalPosition;
		this.QueueFree();
	}
}
