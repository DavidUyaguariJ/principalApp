using EntityModelsHumanTalentApp.Models.App;

namespace HumanTalentApp.Models
{
    public class UserViewModel: AdmnUser
    {
        public List<TAdmnRole> Roles { get; set; }

    }
}
