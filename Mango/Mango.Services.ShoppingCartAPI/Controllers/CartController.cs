using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;
using Mango.Services.ShoppingCartAPI.Service.IService;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _db;
        private IProductService _productService;
        private ICouponService _couponService;

        public CartController(AppDbContext db, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper =  mapper;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartHeader cartHeader = _db.CartHeaders.First(u => u.UserId == userId);
                CartHeaderDto cartHeaderDto = _mapper.Map<CartHeaderDto>(cartHeader);
                IEnumerable<CartDetails> cartDetailsList = _db.CartDetails.Where(u => u.CartHeaderId == cartHeader.CartHeaderId);
                IEnumerable<CartDetailsDto> cartDetailsDtoList = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetailsList);

                CartDto cartDto = new()
                {
                    CartHeader = cartHeaderDto,
                    CartDetails = cartDetailsDtoList,
                };

                IEnumerable<ProductDto> productDtoList = await _productService.GetProducts();

                foreach(CartDetailsDto cartDetailsDto in cartDto.CartDetails)
                {
                    cartDetailsDto.Product = productDtoList.FirstOrDefault(u => u.ProductId == cartDetailsDto.ProductId);
                    cartDto.CartHeader.CartTotal += cartDetailsDto.Product.Price * cartDetailsDto.Count;
                }

                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    if (coupon != null && cartDto.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cartDto.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cartDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                CartHeader cartFromDb = await _db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }
            return _response;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto  cartDto)
        {
            try
            {
                CartHeader? cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if  (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails?.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails?.First());
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    int? productId = cartDto.CartDetails?.First().ProductId;
                    int cartHeaderId = cartHeaderFromDb.CartHeaderId;
                    CartDetails? cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == productId && u.CartHeaderId == cartHeaderId);
                    if (cartDetailsFromDb  == null)
                    {
                        cartDto.CartDetails?.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails?.First());
                        _db.CartDetails.Add(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cartDto.CartDetails?.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails?.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails?.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails?.First());
                        _db.CartDetails.Update(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;

            }
            catch(Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);
                int totalCount = _db.CartHeaders.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                if (totalCount == 1)
                {
                    CartHeader? cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}
