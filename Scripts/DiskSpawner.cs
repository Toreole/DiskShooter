using Godot;
using System;

public partial class DiskSpawner : Node3D
{
	[Export]
	public PackedScene DiskScene { get; set; }

	[Export]
	private Vector3 diskVelocityMinimum, diskVelocityMaximum;

	[Export]
	private float spawnIntervalMin, spawnIntervalMax;
	[Export]
	private float maxAbsDrift;

	private double time = 0;
	private readonly Random random = new();

	private double nextIn = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nextIn = RandomInRange(spawnIntervalMin, spawnIntervalMax);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += delta;
		if (time > nextIn)
		{
			nextIn = RandomInRange(spawnIntervalMin, spawnIntervalMax);

			FlyingTarget node = DiskScene.Instantiate<FlyingTarget>();
			this.AddChild(node);
			node.Position = Position + new Vector3(
				RandomInRange(-4, 4),
				RandomInRange(-4, 4),
				RandomInRange(-4, 4)
				);
			node.LinearVelocity = new Vector3(
				RandomInRange(diskVelocityMinimum.X, diskVelocityMaximum.X),
				RandomInRange(diskVelocityMinimum.Y, diskVelocityMaximum.Y),
				RandomInRange(diskVelocityMinimum.Z, diskVelocityMaximum.Z)
				);
			node.LeftRightDrift = RandomInRange(-maxAbsDrift, maxAbsDrift);
			time = 0;
		}
	}

	private float RandomInRange(float min, float max)
	{
		return random.NextSingle() * (max - min) + min;
	}
}
