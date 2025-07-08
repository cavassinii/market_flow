using System.Security.Claims;

namespace API.Helpers
{
    public static class AuthHelper
    {
        public static string GetSchema(ClaimsPrincipal user)
        {
            var schema = user.FindFirst("schema")?.Value;
            if (string.IsNullOrWhiteSpace(schema))
                throw new Exception("Schema não encontrado no token JWT.");
            return schema;
        }

        public static int GetUsuarioId(ClaimsPrincipal user)
        {
            var id = user.FindFirst("codigo_usuario")?.Value;
            if (string.IsNullOrWhiteSpace(id))
                throw new Exception("Código de usuário não encontrado no token JWT.");
            return int.Parse(id);
        }

        public static string GetUsuarioNome(ClaimsPrincipal user)
        {
            return user.Identity?.Name ?? "";
        }
    }
}
