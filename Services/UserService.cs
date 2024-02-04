using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.Net;

namespace PuppyLearn.Services
{
    public class UserService : IUserService
    {
        private readonly PuppylearnContext _context;
        private readonly IMapper _mapper;
        public UserService(PuppylearnContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ReturnValue> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var res = await _context.Users.FirstOrDefaultAsync(x => (x.UserName == registerDto.UserName) | (x.Email == registerDto.Email));
                    if (res != null)
                    {
                        registerDto.Password = "forbidden";
                        return new ReturnValue
                        {
                            Value = registerDto,
                            HttpCode = HttpStatusCode.BadRequest,
                            Msg = "存在相同用户名或邮箱"
                        };
                    }
                    User newUser = _mapper.Map<User>(registerDto);
                    newUser.PasswordSalt = Hasher.GenerateSalt();
                    newUser.PasswordHash = Hasher.ComputeHash(registerDto.Password, Convert.FromBase64String(newUser.PasswordSalt));
                    newUser.SignUpTime = DateTime.Now;
                    await _context.Users.AddAsync(newUser, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return new ReturnValue { Value = _mapper.Map<UserDto>(newUser), HttpCode = HttpStatusCode.OK, Msg = "注册成功，返回新增用户信息" };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue {
                        Value = registerDto,
                        Msg = "用户取消注册",
                        HttpCode = HttpStatusCode.BadRequest
                    }
                    ;
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = registerDto,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex) {
                return new ReturnValue
                {
                    Value = registerDto,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            } 
        }

        public async Task<ReturnValue> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => (x.UserName == loginDto.UserName) | (x.Email == loginDto.Email), cancellationToken);

                    if (user == null)
                    {
                        return new ReturnValue
                        {
                            Value = _mapper.Map<UserDto>(loginDto),
                            Msg = "用户名或邮箱不存在",
                            HttpCode = HttpStatusCode.BadRequest
                        };
                    }
         
                    var flag = Hasher.VerifyPassword(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PasswordSalt));
                    if (!flag)
                    {
                        return new ReturnValue
                        {
                            Value = _mapper.Map<UserDto>(loginDto),
                            Msg = "密码错误",
                            HttpCode = HttpStatusCode.NotFound
                        };
                    }
                    return new ReturnValue
                    {
                        Value = _mapper.Map<UserDto>(user),
                        Msg = "登录成功",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue {
                        Value = loginDto,
                        Msg = "用户取消登录",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = loginDto,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = loginDto,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}
