using System.Threading.Tasks;
using System;
using System.Collections.Generic;


namespace MLDB.Domain
{
    public interface IUserRepository
    {

        public Task<User> findAsync(UInt16 id);

        public Task<User> findByIdpIdAsync(String idpId);

        public Task<User> insertAsync(User user);
    }
}