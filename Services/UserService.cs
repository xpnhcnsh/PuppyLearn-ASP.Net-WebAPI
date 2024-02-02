using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;

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

        public async Task<ReturnValue> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x=>(x.UserName==registerDto.UserName)|(x.Email==registerDto.Email));
            if (res != null) 
            {
                registerDto.Password = "null";
                return new ReturnValue
                {
                    Value = registerDto,
                    HttpCode = 406,
                    Msg = "存在相同用户名或邮箱"
                };
            }
            User newUser = _mapper.Map<User>(registerDto);
            newUser.PasswordSalt = Hasher.GenerateSalt();
            newUser.PasswordHash = Hasher.ComputeHash(registerDto.Password, Convert.FromBase64String(newUser.PasswordSalt));
            newUser.SignUpTime = DateTime.UtcNow;
            await _context.Users.AddAsync(newUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ReturnValue { Value =_mapper.Map<UserDto>(newUser), HttpCode=200, Msg="注册成功，返回新增用户信息" };
        }

        public async Task<ReturnValue> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => (x.UserName == loginDto.UserName) | (x.Email==loginDto.Email), cancellationToken);

            if (user == null) 
            {
                return new ReturnValue
                {
                    Value = _mapper.Map<UserDto>(loginDto),
                    Msg = "用户名或邮箱不存在",
                    HttpCode = 404
                };
            }
            var flag = Hasher.VerifyPassword(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PasswordSalt));
            if (!flag)
            {
                return new ReturnValue
                {
                    Value = _mapper.Map<UserDto>(loginDto),
                    Msg = "密码错误",
                    HttpCode = 404
                };
            }
            return new ReturnValue
            {
                Value = _mapper.Map<UserDto>(loginDto),
                Msg = "登录成功",
                HttpCode = 200
            };
        }
    }
}
