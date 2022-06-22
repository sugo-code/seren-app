using SerenApp.Core.Model;

namespace SerenApp.Core.Interfaces
{
    public interface IDeviceDataTableRepository
    {
        public Task<IEnumerable<DeviceDataTable>> GetAll();
        public Task<DeviceDataTable> GetById(string id);
        public Task<DeviceDataTable> Insert(DeviceDataTable item);
        public Task<DeviceDataTable> Update(DeviceDataTable item);
        public Task<DeviceDataTable> Delete(DeviceDataTable item);
        Task<int> InsertManyAsync(IEnumerable<DeviceDataTable> devices);
    }
}
