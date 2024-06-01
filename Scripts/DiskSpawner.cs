using Godot;
using System;

public partial class DiskSpawner : Node
{

	[Export]
	public PackedScene DiskScene { get; set; }

	private double time = 0;
	private Random random = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += delta;
		if (time > 4)
		{
			Node3D node = DiskScene.Instantiate<Node3D>();
			this.AddChild(node);
			node.Position = new Vector3(random.Next(-4, 4), random.Next(-4, 4), random.Next(-4, 4));
			time = 0;
		}
	}
}
