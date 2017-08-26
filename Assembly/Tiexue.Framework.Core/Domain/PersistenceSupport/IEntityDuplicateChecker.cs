using Tiexue.Framework.Domain.DomainModel;

namespace Tiexue.Framework.Domain.PersistenceSupport {
    public interface IEntityDuplicateChecker {
        bool DoesDuplicateExistWithTypedIdOf<TId>(IEntityWithTypedId<TId> entity);
    }
}