namespace Android.Dialog
{
    public interface IRootElement
    {
        IGroup Group { get; set; }
        void Add(ISection section);
    }
}