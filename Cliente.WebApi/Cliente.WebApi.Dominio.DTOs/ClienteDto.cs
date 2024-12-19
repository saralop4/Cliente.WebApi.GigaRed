namespace Cliente.WebApi.Dominio.DTOs
{
    public class ClienteDto
    {
        public long IdCliente { get; set; }
        public long IdPersona { get; set; }
        public bool? Estado { get; set; }
        public string? UsuarioQueRegistra { get; set; }
        public string? UsuarioQueActualiza { get; set; }
        public string? IpDeRegistro { get; set; }
        public string? IpDeActualizado { get; set; }
        public int IdIndicativo { get; set; }
        public int IdCiudad { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Telefono { get; set; }
    }
   
}
