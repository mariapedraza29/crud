namespace crud.Repositorio
{
    public class RepositorioProducto
    {
        private readonly string cadenaSQL;

        public RepositorioProducto(IConfiguration config)
        {

            cadenaSQL = config.GetConnectionString("cadenaSQL");

        }
    }
}
