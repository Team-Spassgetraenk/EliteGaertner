using Contracts.Data_Transfer_Objects;

namespace DataManagement.Interfaces;

public interface IProfileDbs
{

    //Gibt ein P
    public ProfileDto GetProfile(int userId);

}