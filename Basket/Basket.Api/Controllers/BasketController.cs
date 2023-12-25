﻿using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    [HttpGet("{userName}", Name = "Getbasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _basketRepository.GetBasket(userName);

        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        return Ok(await _basketRepository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
    {
        await _basketRepository.DeleteBasket(userName);
        return Ok();
    }
}
