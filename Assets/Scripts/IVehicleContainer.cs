using System.Collections.Generic;

public interface IVehicleContainer
{
    public void AddVehicle(VehiclePanel vehicle);

    public void RemoveVehicle(VehiclePanel vehicle);
        
    public List<VehiclePanel> GetVehicles();
}