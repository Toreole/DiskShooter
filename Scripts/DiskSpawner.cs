using Godot;
using System;

public partial class DiskSpawner : Node3D
{

	[Export]
	public PackedScene DiskScene { get; set; }

	private double time = 0;
	private Random random = new Random();

	private double nextIn = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nextIn = random.NextDouble() * 2d + 1d;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += delta;
		if (time > nextIn)
		{
			nextIn = random.NextDouble() * 2d + 1d;
			FlyingTarget node = DiskScene.Instantiate<FlyingTarget>();
			this.AddChild(node);
			node.Position = Position + new Vector3(random.Next(-4, 4), random.Next(-4, 4), random.Next(-4, 4));
			node.LinearVelocity = new Vector3(random.Next(-2, 2), random.Next(3, 6), random.Next(6, 20));
			node.LeftRightDrift = (float)random.NextDouble() * 20f - 10f;
			time = 0;
		}
	}
}
