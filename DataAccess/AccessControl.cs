abstract class AccessControl<T>
{
    static string path = string.Empty;
    public abstract List<T> LoadAll();
    public abstract void WriteAll(List<T> list);
}