using projeto.Models;

namespace projeto.Repositories.Interfaces
{
    public interface IItemRepository
    {
        public IEnumerable<Item> Itens{get;}
        public IEnumerable<Item> ItensEmDestaque{get;}
        public Item GetItemById(int itemId);
    }
}