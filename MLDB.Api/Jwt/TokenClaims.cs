namespace MLDB.Api.Jwt
{
    public sealed class TokenClaims { 
        public const string EMAIL_CLAIMTYPE =  "https://mldb/claims/email";
        public const string EMAIL_VERIFIED_CLAIMTYPE =  "https://mldb/claims/email_verified";
        public const string NAME_CLAIMTYPE =  "https://mldb/claims/name";
    }

}