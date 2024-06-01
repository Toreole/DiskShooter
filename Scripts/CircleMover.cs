using Godot;
using System;

public partial class CircleMover : Node3D
{
	[Export]
#nullable enable
	public GpuParticles3D? ParticleSystem { get; set; } = null;

	Vector3 origin;
	float angle;
	float time = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		origin = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 offset = new Vector3(2, 0, 0);
		offset = offset.Rotated(Vector3.Up, angle);
		angle += (float)delta;
		Position = origin + offset;

		time += (float)delta;
		if(time > 4)
		{
			time = 0;
			GD.Print("log:", ParticleSystem);
			if(ParticleSystem != null) ParticleSystem.Emitting = true;
		}

	}
}
