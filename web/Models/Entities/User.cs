using web.Common.Enums;
using web.Models.Parameters;

namespace web.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Group Group { get; set; }
        public string Description { get; set; }

        public User(int id, string name, Group group, string description)
        {
            Id = id;
            Name = name;
            Group = group;
            Description = description;
        }

        public User(UserParamModel param)
        {
            // AutoMapper(Nuget)を使うとこの処理が要らなくなる
            Id = param.Id;
            Name = param.Name;
            Group = param.Group;
            Description = param.Description;
        }
    }
}
