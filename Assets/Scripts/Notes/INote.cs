public interface INote
{
    public void Hit(int lane);
    public void Miss();
    public void SetInRange(bool inRange);
    public bool GetInRange();
}
