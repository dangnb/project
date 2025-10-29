using Asp.Versioning;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Enumerations;
using MNG.Contract.Extensions;
using MNG.Contract.Services.V1.Product;
using MNG.Persentation.Abtractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MNG.Persentation.Controllers.V1;

[ApiVersion(1)]
public class ProductsController : ApiController
{
    private readonly ILogger<ProductsController> _logger;       

    public ProductsController(ISender sender, ILogger<ProductsController> logger) : base(sender: sender)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetProducts")]
    [ProducesResponseType(typeof(Result<IEnumerable<Response.ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Products(string? searchTerm, 
        string? sortColumn,
        string? sortOrder,
        string? sortColumnAndOrder,
        int pageIndex = 1,
        int pageSize = 10)
    {
        SortOrder sort = SortOrderExtension.ConvertStringToSortOder(sortOrder);
        IDictionary<string, SortOrder> _sortColumnAndOrder = SortOrderExtension.ConvertStringToSortOderV2(sortColumnAndOrder);
        Result<PagedResult<Response.ProductResponse>> result = await Sender.Send(new Query.GetProductsQuery(searchTerm, sortColumn, sort, _sortColumnAndOrder, pageIndex, pageSize));
        return Ok(result);
    }

    [HttpGet("{productId}", Name = "GetByIdProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdProduct(Guid Id)
    {
        var result = await Sender.Send(new Query.GetPrductByIdQuery(Id));
        return Ok(result);
    }

    [HttpPost(Name = "CreateProducts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateProducts([FromBody] Command.CreateProductCommand CreateProduct)
    {
        var result = await Sender.Send(CreateProduct);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Ok(result);
    }

    [HttpPut("{productId}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct([FromBody] Command.UpdateProductCommand updateProductCommand, Guid productId)
    {
        var productCommand = new Command.UpdateProductCommand(productId, updateProductCommand.Name, updateProductCommand.Price, updateProductCommand.Description);
        var result = await Sender.Send(productCommand);
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Ok(result);
    }

    [HttpDelete("{productId}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        Result result = await Sender.Send(new Command.DeleteProductCommand(productId));
        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        return Ok(result);
    }
}
