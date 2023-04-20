namespace ABBAPI
{
    public class Usuario
    {
        public string Nombre { get; set; } = null!;
        public string PApellido { get; set; } = null!;
        public string SApellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public int Age { get; set; }
        public string Rol { get; set; } = null!;
    }
}
