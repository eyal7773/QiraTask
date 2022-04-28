namespace Dal
{
    public interface IRepo<T>
    {
        void Add(T record);
        DataWrapper<T> GetAll(int pageLength = 10, int startRecord = 0, string sort = "", string filterColumn = "", string filterTerm = "");
        T GetById(int id);
        void Update(T newData, int id);
    }
}