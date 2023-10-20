using projeto.Models;

namespace projeto.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        public IEnumerable<Categoria> Categorias{get;}
    }
}