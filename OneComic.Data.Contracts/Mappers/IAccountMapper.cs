using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public interface IAccountMapper
    {
        DTO.Account ToDTO(Account account);
        Account ToEntity(DTO.Account account);
    }
}
