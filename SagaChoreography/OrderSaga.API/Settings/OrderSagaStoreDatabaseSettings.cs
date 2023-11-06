namespace OrderSaga.API.Context
{
    public class OrderSagaStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CoursesCollectionName { get; set; } = null!;
    }
}
