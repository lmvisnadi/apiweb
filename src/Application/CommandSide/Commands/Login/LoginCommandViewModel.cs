namespace Application.CommandSide.Commands.Login
{
    public struct LoginCommandViewModel
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
        public Guid UsuarioId { get; set; }

        public LoginCommandViewModel(string token, Guid refreshToken, Guid usuarioId)
        {
            Token = token;
            RefreshToken = refreshToken;
            UsuarioId = usuarioId;
        }
    }
}
