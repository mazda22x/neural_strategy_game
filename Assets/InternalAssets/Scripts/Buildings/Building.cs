public interface IBuilding
{
	public int Health { get; set; }
	public int Income { get; set; }

	public void Damage(int points) { }
	public void Heal(int points) { }

}