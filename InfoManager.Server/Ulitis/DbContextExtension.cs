using InfoManager.Server.Models;
using InfoManager.Server.Services;

using OneOf;

namespace InfoManager.Server.Ulitis
{
    public static class DbContextExtension
    {

        public struct SpaceNotFound { }
        public struct MemberNotFound { }
        public static async Task<OneOf<(SpaceMember, Space), SpaceNotFound, MemberNotFound>> GetMember(this MainDbUnitOfWork unitOfWork,User user, int spaceId)
        {
            var space = await unitOfWork.SpaceRepository.FindAsync(spaceId);
            if (space is null)
            {
                return new SpaceNotFound();
            }
            SpaceMember? member = await unitOfWork.SpaceMemberRepository.FindAsync(space, user);
            if (member is null)
            {
                return new MemberNotFound();
            }
            return (member, space);
        }

        public static string GetTblName(Table tbl) => $"info_tb_{tbl.Id:x}";
        public static string GetFieldName(TableField field) => $"f_{field.Id:x}";
    }
}
