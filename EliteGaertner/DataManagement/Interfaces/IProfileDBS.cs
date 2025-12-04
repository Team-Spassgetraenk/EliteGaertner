using Contracts.Data_Transfer_Objects;

namespace Infrastructure.Interfaces;

public interface IProfileDBS
{

    //Gibt ein P
    public ProfileDto GetProfile(int userId);

}