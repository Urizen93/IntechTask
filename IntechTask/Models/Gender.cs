namespace IntechTask.Models
{
    public sealed class Gender
    {
        public Gender(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; }

        public string Name { get; }
    }
}