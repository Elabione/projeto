using projeto.Models;
namespace projeto.ViewModel
{
public class PedidoItensViewModel
{
public Pedido Pedidos {get;set;}
public IEnumerable<PedidoItem> PedidoItens {get;set;}
}
}