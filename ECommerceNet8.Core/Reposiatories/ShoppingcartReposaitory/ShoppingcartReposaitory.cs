using ECommerceNet8.Core.DTOS.ShoppingCartDto.Models;
using ECommerceNet8.Core.DTOS.ShoppingCartDto.Request;
using ECommerceNet8.Core.DTOS.ShoppingCartDto.Response;
using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using ECommerceNet8.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerceNet8.Core.DtosConvertions;
using ECommerceNet8.DTOConvertions;

namespace ECommerceNet8.Core.Reposiatories.ShoppingcartReposaitory
{
    public class ShoppingcartReposaitory 
    {

        private readonly ApplicationDbContext _db;

        public ShoppingcartReposaitory(ApplicationDbContext db)
        {
            _db = db;
        }

       // public async Task<Response_ShoppingCart> GetAllCartItems(string userId)
        //{
        //    bool anyCantBeSold = false;
        //    decimal totalPrice = 0;

        //    var userCart = await _db.ShoppingCarts
        //        .Include(sc => sc.CartItems)
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);
        //    if (userCart == null)
        //    {
        //        return new Response_ShoppingCart()
        //        {
        //            CanBeSold = false,
        //            TotalPrice = 0,
        //            Message = "No Shopping Cart Found For This User"
        //        };
        //    }

        //    Response_ShoppingCart response_ShoppingCart = new Response_ShoppingCart();
        //    response_ShoppingCart.ShoppingCartId = userCart.Id;

        //    var userCartItems = userCart.CartItems.ToList();
        //    int shoppingCartId = userCart.Id;

        //    foreach (var item in userCartItems)
        //    {
        //        Model_CartItemReturn cartItemReturn = new Model_CartItemReturn();

        //        var existingProductVariant = await _db.productVariants
        //            .Include(pv => pv.ProductSize)
        //            .Include(pv => pv.ProductColor)
        //            .FirstOrDefaultAsync(pv => pv.Id == item.ProductVariantId);

        //        if (existingProductVariant == null)
        //        {
        //            cartItemReturn.CanBeSold = false;
        //            cartItemReturn.ProductVariantId = item.ProductVariantId;
        //            cartItemReturn.SelectedQuantity = item.Quantity;
        //            cartItemReturn.Message = "No Item Found With Given Id";
        //            response_ShoppingCart.ItemsCanBeSold.Add(cartItemReturn);

        //            anyCantBeSold = true;
        //        }
        //        else
        //        {
        //            var baseProduct = await _db.BaseProducts
        //                .FirstOrDefaultAsync(bp => bp.Id == existingProductVariant.BaseProductId);

        //            //CONVERT TO DTO
        //            cartItemReturn = baseProduct.ConvertToDtoCartItem(existingProductVariant, item);

        //            if (cartItemReturn.CanBeSold == false)
        //            {
        //                response_ShoppingCart.ItemsCantBeSold.Add(cartItemReturn);
        //            }
        //            else
        //            {
        //                totalPrice += cartItemReturn.TotalPrice;
        //                response_ShoppingCart.ItemsCanBeSold.Add(cartItemReturn);
        //            }

        //        }
        //    }

        //    if (anyCantBeSold == true)
        //    {
        //        response_ShoppingCart.CanBeSold = false;
        //        response_ShoppingCart.Message = "Some Items Cant Be Sold";
        //        response_ShoppingCart.TotalPrice = 0;
        //    }
        //    else
        //    {
        //        response_ShoppingCart.CanBeSold = true;
        //        response_ShoppingCart.Message = "All Items Good For Sale";
        //        response_ShoppingCart.TotalPrice = totalPrice;
        //    }

        //    return response_ShoppingCart;
        //}

        //public async Task<Response_ShoppingCartInfo> AddItem(string userId, Request_ShoppingCart shoppingCartItem)
        //{
        //    var userCart = await _db.ShoppingCarts
        //        .Include(sc => sc.CartItems)
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);
        //    if (userCart == null)
        //    {
        //        ShoppingCart shoppingCart = new ShoppingCart();
        //        shoppingCart.ApplicationUserId = userId;
        //        await _db.ShoppingCarts.AddAsync(shoppingCart);
        //        await _db.SaveChangesAsync();

        //        userCart = await _db.ShoppingCarts
        //        .Include(sc => sc.CartItems)
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);
        //    }

        //    var cartItem = new CartItem();
        //    cartItem.ProductVariantId = shoppingCartItem.ProductVariantId;

        //    //GET AND CHECK PRODUCT VARIANT
        //    var productVariant = await _db.productVariants
        //        .FirstOrDefaultAsync(pv => pv.Id == shoppingCartItem.ProductVariantId);
        //    if (productVariant == null)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = 0,
        //            Message = "No Product Variant Found With Given Id",
        //            RequestQty = 0,
        //            StoreQty = 0
        //        };
        //    }

        //    //CHECK IF CART ITEM ALREADY EXIST
        //    var cartItemInDb = await _db.CartItems
        //        .AnyAsync(ci => ci.ShoppingCartId == userCart.Id
        //        && ci.ProductVariantId == productVariant.Id);
        //    if (cartItemInDb == true)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = productVariant.Id,
        //            Message = "This Item Already In The Cart",
        //            RequestQty = 0,
        //            StoreQty = 0
        //        };
        //    }
        //    //CHECK AVAILABLE QUANTITY
        //    if (productVariant.Quantity < shoppingCartItem.Quantity)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = productVariant.Id,
        //            Message = "Not Enough Items In Storage",
        //            RequestQty = shoppingCartItem.Quantity,
        //            StoreQty = productVariant.Quantity
        //        };
        //    }

        //    cartItem.Quantity = shoppingCartItem.Quantity;
        //    cartItem.ShoppingCartId = userCart.Id;

        //    await _db.CartItems.AddAsync(cartItem);
        //    await _db.SaveChangesAsync();

        //    return new Response_ShoppingCartInfo()
        //    {
        //        IsSuccess = true,
        //        ProductVariantId = productVariant.Id,
        //        Message = "Product Added To The Cart",
        //        RequestQty = shoppingCartItem.Quantity,
        //        StoreQty = productVariant.Quantity
        //    };


        //}
        //public async Task<Response_ShoppingCartInfo> UpdateQty(string userId, Request_ShoppingCart shoppingCartItem)
        //{
        //    //CHECK QUANTIITY
        //    if (shoppingCartItem.Quantity <= 0)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = shoppingCartItem.ProductVariantId,
        //            Message = "Cannot Change to zero or less, delete if needed",
        //            RequestQty = shoppingCartItem.Quantity,
        //            StoreQty = 0
        //        };
        //    }
        //    //GET AND CHECK CART ITEMS
        //    var userCart = await _db.ShoppingCarts
        //        .Include(sc => sc.CartItems)
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);

        //    var productVariant = await _db.productVariants
        //        .FirstOrDefaultAsync(pv => pv.Id == shoppingCartItem.ProductVariantId);

        //    var cartItem = await _db.CartItems
        //        .Where(ci => ci.ShoppingCartId == userCart.Id
        //        && ci.ProductVariantId == shoppingCartItem.ProductVariantId)
        //        .FirstOrDefaultAsync();

        //    if (cartItem == null || productVariant == null)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = shoppingCartItem.ProductVariantId,
        //            Message = "No items found with Given Id",
        //            RequestQty = shoppingCartItem.Quantity,
        //            StoreQty = 0
        //        };
        //    }
        //    //CHECK IF ENOUGH ITEMS
        //    if (shoppingCartItem.Quantity > productVariant.Quantity)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = productVariant.Id,
        //            Message = "Not Enough Items In Storage",
        //            RequestQty = shoppingCartItem.Quantity,
        //            StoreQty = productVariant.Quantity
        //        };
        //    }
        //    //CHANGE QUANTITY
        //    cartItem.Quantity = shoppingCartItem.Quantity;
        //    await _db.SaveChangesAsync();

        //    return new Response_ShoppingCartInfo()
        //    {
        //        IsSuccess = true,
        //        ProductVariantId = productVariant.Id,
        //        Message = "Quantity Updated",
        //        RequestQty = shoppingCartItem.Quantity,
        //        StoreQty = productVariant.Quantity
        //    };

        //}
        //public async Task<Response_ShoppingCartInfo> RemoveItem(string userId, Request_ShoppingCart shoppingCartItem)
        //{
        //    var shoppingCart = await _db.ShoppingCarts
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);

        //    var cartItem = await _db.CartItems
        //        .FirstOrDefaultAsync(ci => ci.ShoppingCartId == shoppingCart.Id
        //        && ci.ProductVariantId == shoppingCartItem.ProductVariantId);
        //    if (cartItem == null)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = 0,
        //            Message = "No cart item exist with given Id",
        //            RequestQty = shoppingCartItem.Quantity,
        //            StoreQty = 0
        //        };
        //    }

        //    _db.CartItems.Remove(cartItem);
        //    await _db.SaveChangesAsync();

        //    return new Response_ShoppingCartInfo()
        //    {
        //        IsSuccess = true,
        //        ProductVariantId = cartItem.ProductVariantId,
        //        Message = "Cart item removed",
        //        RequestQty = 0,
        //        StoreQty = 0,
        //    };
        //}

        //public async Task<Response_ShoppingCartInfo> ClearCart(string userId)
        //{
        //    var shoppingCart = await _db.ShoppingCarts
        //        .Include(sc => sc.CartItems)
        //        .FirstOrDefaultAsync(sc => sc.ApplicationUserId == userId);

        //    if (shoppingCart == null)
        //    {
        //        return new Response_ShoppingCartInfo()
        //        {
        //            IsSuccess = false,
        //            ProductVariantId = 0,
        //            Message = "No Shopping Cart Found For This User",
        //            RequestQty = 0,
        //            StoreQty = 0,
        //        };
        //    }


        //    foreach (var cartItem in shoppingCart.CartItems)
        //    {
        //        _db.CartItems.Remove(cartItem);
        //    }

        //    await _db.SaveChangesAsync();

        //    return new Response_ShoppingCartInfo()
        //    {
        //        IsSuccess = true,
        //        ProductVariantId = 0,
        //        Message = "All Cart Items Removed",
        //        RequestQty = 0,
        //        StoreQty = 0,
        //    };
        //}
    }

}

