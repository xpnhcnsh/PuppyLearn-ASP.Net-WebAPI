using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.Net;

namespace PuppyLearn.Services
{
    public class UserService : IUserService
    {
        private readonly PuppyLearnContext _context;
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public UserService(PuppyLearnContext context, IMapper mapper, IBookService bookService)
        {
            _context = context;
            _mapper = mapper;
            _bookService = bookService;
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
                    return new ReturnValue
                    {
                        Value = registerDto,
                        Msg = "用户取消注册",
                        HttpCode = HttpStatusCode.BadRequest
                    };
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
            catch (Exception ex)
            {
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
                    var user = await _context.Users.Where(x => x.Email.Equals(loginDto.Email)).SingleOrDefaultAsync();

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
                    var userDto = _mapper.Map<UserDto>(user);
                    if (userDto.Settings == null)
                    {
                        userDto.Settings = JsonConvert.SerializeObject(Global.GetDefaultUserSettings());
                    }

                    return new ReturnValue
                    {
                        Value = userDto,
                        Msg = "登录成功",
                        HttpCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
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

        public async Task<ReturnValue> UpdateSelectedBooksAsync(Guid userId, List<BookDto> bookDtoList, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    int failCount = 0;
                    int addCount = 0;
                    int removeCount = 0;
                    var newBookIdList = bookDtoList.Select(x => x.Id).ToList();
                    var existingBookIdList = _context.UserBooks.Where(x => x.UserId == userId && x.Finished == false).Select(x => x.BookId).ToList();
                    var duplicatedBookIdList = newBookIdList.Intersect(existingBookIdList).ToList();
                    var tobeAddedBookIdList = newBookIdList.Except(existingBookIdList).ToList();
                    var tobeRemovedBookIdList = existingBookIdList.Except(newBookIdList).ToList();

                    // 先删除
                    if (tobeRemovedBookIdList.Count > 0)
                    {
                        var tobeRemovedUserBookList = await _context.UserBooks.Where(x => (x.UserId == userId) && (tobeRemovedBookIdList.Contains(x.BookId))).ToListAsync();
                        _context.UserBooks.RemoveRange(tobeRemovedUserBookList);
                        var progress = await _context.Progresses.Where(x=>x.UserId == userId && tobeRemovedBookIdList.Contains(x.BookId)).ToListAsync();
                        _context.Progresses.RemoveRange(progress);
                        removeCount++;
                    }
                    // 再添加
                    if (tobeAddedBookIdList.Count > 0)
                    {
                        List<UserBook> newEntries = new List<UserBook>();
                        foreach (var bookId in tobeAddedBookIdList)
                        {
                            UserBook newBook = new UserBook()
                            {
                                UserId = userId,
                                BookId = bookId,
                                Finished = false,
                                StartDateTime = DateTime.Now,
                                WordsDone = 0,
                                RepeatTimes = 0,
                                LastUpdateTime = DateTime.Now,
                                Id = Guid.NewGuid(),
                            };
                            newEntries.Add(newBook);
                            addCount++;
                        }
                        await _context.UserBooks.AddRangeAsync(newEntries);
                    }
                    // 对于重复的书，如果已经完成学习，将已完成的状态改成未完成，且开始学习时间改成当前时间；如果是未完成状态，提示请勿重复添加。
                    if (duplicatedBookIdList.Count > 0)
                    {
                        foreach (var bookId in duplicatedBookIdList)
                        {
                            var userBookFromDb = await _bookService.GetUserBookById(bookId, userId, cancellationToken);
                            if (userBookFromDb.Value.Count > 0 && userBookFromDb.Value[0].Finished == true)
                            {
                                userBookFromDb.Value[0].Finished = false;
                                userBookFromDb.Value[0].StartDateTime = DateTime.Now;
                            }
                            else if (userBookFromDb.Value.Count > 0 && userBookFromDb.Value[0].Finished == false)
                            {
                                failCount++;
                            }
                        }
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    return new ReturnValue
                    {
                        Value = $"成功:{addCount};重复:{failCount};取消:{removeCount}",
                        Msg = "成功添加",
                        HttpCode = HttpStatusCode.OK,
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ReturnValue> GetUserBooksAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    var res = await _context.UserBooks.Include(x => x.Book).Where(x => x.UserId == userId).ToListAsync();
                    return new ReturnValue
                    {
                        Value = res,
                        Msg = "返回该用户当前所选books",
                        HttpCode = HttpStatusCode.OK,
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }

        /// <summary>
        /// 如果route中有wordName参数，就表示只是查询某本书里的一个单词；如果要查询某本书的所有单词，在route中不要携带wordName即可。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="totalRecords"></param>
        /// <param name="wordName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ReturnValue> GetWordsFromABookAsync(Guid userId, Guid bookId, int skip, int take, int totalRecords, string? wordName, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (wordName != null)
                    {
                        var wordsList = await _context.Words.AsNoTracking().Where(x => x.BookId == bookId && x.WordName.Contains(wordName))
                            .OrderBy(x => x.WordName)
                            .Select(x => new WordDto
                            {
                                Id = x.Id,
                                WordName = x.WordName,
                                BookId = bookId,
                                BookNameCh = x.Book.BookNameCh,
                                Ukphone = x.Ukphone,
                                Usphone = x.Usphone,
                                Ukspeech = x.Ukspeech,
                                Usspeech = x.Usspeech,
                                Phone = x.Phone,
                                Speech = x.Speech,
                                Cognates = _mapper.Map<List<CognatesDto>>(x.Cognates),
                                Phrases = _mapper.Map<List<PhraseDto>>(x.Phrases),
                                RemMethods = _mapper.Map<List<RemMethodDto>>(x.RemMethods),
                                Sentences = _mapper.Map<List<SentenceDto>>(x.Sentences),
                                SingleChoiceQuestions = _mapper.Map<List<SingleChoiceQuestionDto>>(x.SingleChoiceQuestions),
                                Synonymous = _mapper.Map<List<SynonymousDto>>(x.Synonymous),
                                Trans = _mapper.Map<List<TranDto>>(x.Trans)
                            })
                        .ToListAsync(cancellationToken);
                        var res = new PagedResponseDto<WordDto>(skip, take, wordsList.Count, wordsList);
                        return new ReturnValue
                        {
                            Msg = $"Return {wordsList.Count} words from {res.Data.Select(x => x.BookNameCh).Distinct().First()}",
                            HttpCode = HttpStatusCode.OK,
                            Value = res,
                        };
                    }
                    else if ((skip < 0 || take <= 0 || totalRecords <= 0) && wordName == null)
                    {
                        return new ReturnValue
                        {
                            Msg = " One of parameters is smaller than 0!",
                            Value = $"{nameof(skip)}:{take}; {nameof(skip)}:{take}; {nameof(totalRecords)}:{totalRecords}.",
                            HttpCode = HttpStatusCode.BadRequest

                        };
                    }
                    else
                    {
                        var wordsList = await _context.Words.AsNoTracking().Where(x => x.BookId == bookId)
                        .OrderBy(x => x.WordName).Skip(skip)
                        .Take(take)
                        .Select(x => new WordDto
                        {
                            Id = x.Id,
                            WordName = x.WordName,
                            BookId = bookId,
                            BookNameCh = x.Book.BookNameCh,
                            Ukphone = x.Ukphone,
                            Usphone = x.Usphone,
                            Ukspeech = x.Ukspeech,
                            Usspeech = x.Usspeech,
                            Phone = x.Phone,
                            Speech = x.Speech,
                            Cognates = _mapper.Map<List<CognatesDto>>(x.Cognates),
                            Phrases = _mapper.Map<List<PhraseDto>>(x.Phrases),
                            RemMethods = _mapper.Map<List<RemMethodDto>>(x.RemMethods),
                            Sentences = _mapper.Map<List<SentenceDto>>(x.Sentences),
                            SingleChoiceQuestions = _mapper.Map<List<SingleChoiceQuestionDto>>(x.SingleChoiceQuestions),
                            Synonymous = _mapper.Map<List<SynonymousDto>>(x.Synonymous),
                            Trans = _mapper.Map<List<TranDto>>(x.Trans)
                        })
                        .ToListAsync(cancellationToken);
                        var res = new PagedResponseDto<WordDto>(skip, take, totalRecords, wordsList);
                        return new ReturnValue
                        {
                            Msg = $"Return {take} words from {res.Data.Select(x => x.BookNameCh).Distinct().First()}, skip: {skip}",
                            HttpCode = HttpStatusCode.OK,
                            Value = res,
                        };
                    }
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId: {bookId}; pageSize: {take}; skip:{skip}",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }

        /// <summary>
        /// 从所有书中，返回查询的单词
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wordName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ReturnValue> GetOneWordFromAllBookAsync(Guid userId, string wordName, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested) 
                {
                    var res = await _context.Words.Where(x=>x.WordName == wordName).Select(x=>new WordDto
                    {
                        Id = x.Id,
                        WordName = x.WordName,
                        BookId = x.BookId,
                        BookNameCh = x.Book.BookNameCh,
                        Ukphone = x.Ukphone,
                        Usphone = x.Usphone,
                        Ukspeech = x.Ukspeech,
                        Usspeech = x.Usspeech,
                        Phone = x.Phone,
                        Speech = x.Speech,
                        Cognates = _mapper.Map<List<CognatesDto>>(x.Cognates),
                        Phrases = _mapper.Map<List<PhraseDto>>(x.Phrases),
                        RemMethods = _mapper.Map<List<RemMethodDto>>(x.RemMethods),
                        Sentences = _mapper.Map<List<SentenceDto>>(x.Sentences),
                        SingleChoiceQuestions = _mapper.Map<List<SingleChoiceQuestionDto>>(x.SingleChoiceQuestions),
                        Synonymous = _mapper.Map<List<SynonymousDto>>(x.Synonymous),
                        Trans = _mapper.Map<List<TranDto>>(x.Trans)
                    }).ToListAsync(cancellationToken);
                    if (res.Count > 0)
                    {
                        return new ReturnValue
                        {
                            Msg = $"Return '{wordName}' from <{string.Join(" ,", res.Select(x => x.BookNameCh))}>",
                            HttpCode = HttpStatusCode.OK,
                            Value = res,
                        };
                    }
                    else
                    {
                        return new ReturnValue
                        {
                            Msg = $"'{wordName}' not found!",
                            HttpCode = HttpStatusCode.NoContent,
                            Value = res,
                        };
                    }
                    
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; wordName: {wordName};",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }

        }

        public async Task<ReturnValue> UpdateAWordAsync(Guid userId, Guid bookId, WordNFieldsDto wordNFieldsDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    WordDto wordDto = wordNFieldsDto.WordDto;
                    List<string> updateFields = wordNFieldsDto.Fields;
                    var toUpdate = await _context.Words.Where(x => x.BookId == bookId && x.Id == wordDto.Id)
                        .Include(x => x.Trans)
                        .Include(x => x.Phrases)
                        .Include(x => x.Synonymous)
                        .Include(x => x.Cognates)
                        .Include(x => x.Sentences)
                        .Include(x => x.RemMethods)
                        .Include(x => x.SingleChoiceQuestions)
                        .Include(x => x.Book)
                        .Select(x => x).SingleOrDefaultAsync(cancellationToken);
                    if (toUpdate == null)
                    {
                        return new ReturnValue
                        {
                            Value = $"userId: {userId}; bookId:{bookId}; wordDto:{wordDto}",
                            Msg = "Word does not exist, add it first",
                            HttpCode = HttpStatusCode.BadRequest
                        };
                    }
                    else
                    {
                        foreach (string field in updateFields)
                        {
                            if (field == "wordName")
                            {
                                toUpdate.WordName = wordDto.WordName;
                            }
                            else if (field == "usphone")
                            {
                                toUpdate.Usphone = wordDto.Usphone;
                            }
                            else if (field == "ukphones")
                            {
                                toUpdate.Ukphone = wordDto.Ukphone;
                            }
                            else if (field == "usspeech")
                            {
                                toUpdate.Usspeech = wordDto.Usspeech;
                            }
                            else if (field == "ukspeech")
                            {
                                toUpdate.Ukspeech = wordDto.Ukspeech;
                            }
                            else if (field == "ukspeech")
                            {
                                toUpdate.Ukspeech = wordDto.Ukspeech;
                            }
                            else if (field.StartsWith("transCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid tranId))
                                {
                                    toUpdate.Trans.Where(x => x.Id == tranId).Select(x => x).Single().TransCn = wordDto.Trans.Where(x => x.Id == tranId).Select(x => x.TransCn).Single()!;
                                }
                                else
                                {
                                    wordDto.Trans.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var tempTran = new Tran
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId!,
                                            TransCn = x.TransCn!,
                                            TransEn = x.TransEn!,
                                            Pos = x.Pos!
                                        };
                                        toUpdate.Trans.Add(tempTran);
                                    });
                                }
                            }
                            else if (field.StartsWith("transEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid tranId))
                                {
                                    toUpdate.Trans.Where(x => x.Id == tranId).Select(x => x).Single().TransEn = wordDto.Trans.Where(x => x.Id == tranId).Select(x => x.TransEn).Single()!;
                                }
                            }
                            else if (field.StartsWith("transPos"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid tranId))
                                {
                                    toUpdate.Trans.Where(x => x.Id == tranId).Select(x => x).Single().Pos = wordDto.Trans.Where(x => x.Id == tranId).Select(x => x.Pos).Single()!;
                                }
                            }
                            else if (field.StartsWith("phraseEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid phraseId))
                                {
                                    toUpdate.Phrases.Where(x => x.Id == phraseId).Select(x => x).Single().PhraseEn = wordDto.Phrases.Where(x => x.Id == phraseId).Select(x => x.PhraseEn).Single()!;
                                }
                                else
                                {
                                    wordDto.Phrases.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var tempPhrase = new Phrase
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId,
                                            PhraseEn = x.PhraseEn!,
                                            PhraseCn = x.PhraseCn!
                                        };
                                        toUpdate.Phrases.Add(tempPhrase);
                                    });
                                }
                            }
                            else if (field.StartsWith("phraseCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid phraseId))
                                {
                                    toUpdate.Phrases.Where(x => x.Id == phraseId).Select(x => x).Single().PhraseCn = wordDto.Phrases.Where(x => x.Id == phraseId).Select(x => x.PhraseCn).Single()!;
                                }
                            }
                            else if (field.StartsWith("synoEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid synoId))
                                {
                                    toUpdate.Synonymous.Where(x => x.Id == synoId).Select(x => x).Single().SynoEn = wordDto.Synonymous.Where(x => x.Id == synoId).Select(x => x.SynoEn).Single()!;
                                }
                                else
                                {
                                    wordDto.Synonymous.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var tempSyno = new Synonymou
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId,
                                            Pos = x.Pos!,
                                            TransCn = x.TransCn!,
                                            SynoEn = x.SynoEn!
                                        };
                                        toUpdate.Synonymous.Add(tempSyno);
                                    });
                                }

                            }
                            else if (field.StartsWith("synoCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid synoId))
                                {
                                    toUpdate.Synonymous.Where(x => x.Id == synoId).Select(x => x).Single().TransCn = wordDto.Synonymous.Where(x => x.Id == synoId).Select(x => x.TransCn).Single()!;
                                }
                            }
                            else if (field.StartsWith("synoPos"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid synoId))
                                {
                                    toUpdate.Synonymous.Where(x => x.Id == synoId).Select(x => x).Single().Pos = wordDto.Synonymous.Where(x => x.Id == synoId).Select(x => x.Pos).Single()!;
                                }
                            }
                            else if (field.StartsWith("cognateEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid cognateId))
                                {
                                    toUpdate.Cognates.Where(x => x.Id == cognateId).Select(x => x).Single().CognateEn = wordDto.Cognates.Where(x => x.Id == cognateId).Select(x => x.CognateEn).Single()!;
                                }
                                else
                                {
                                    wordDto.Cognates.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var tempTran = new Cognate
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId!,
                                            CognateCn = x.CognateCn!,
                                            CognateEn = x.CognateEn!,
                                            Pos = x.Pos!
                                        };
                                        toUpdate.Cognates.Add(tempTran);
                                    });
                                }
                            }
                            else if (field.StartsWith("cognateCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid cognateId))
                                {
                                    toUpdate.Cognates.Where(x => x.Id == cognateId).Select(x => x).Single().CognateCn = wordDto.Cognates.Where(x => x.Id == cognateId).Select(x => x.CognateCn).Single()!;
                                }
                            }
                            else if (field.StartsWith("cognatePos"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid cognateId))
                                {
                                    toUpdate.Cognates.Where(x => x.Id == cognateId).Select(x => x).Single().Pos = wordDto.Cognates.Where(x => x.Id == cognateId).Select(x => x.Pos).Single()!;
                                }
                            }
                            else if (field.StartsWith("sentenceEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid sentenceId))
                                {
                                    toUpdate.Sentences.Where(x => x.Id == sentenceId).Select(x => x).Single().SentenceEn = wordDto.Sentences.Where(x => x.Id == sentenceId).Select(x => x.SentenceEn).Single()!;
                                }
                                else
                                {
                                    wordDto.Sentences.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var sentTemp = new Sentence
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId!,
                                            SentenceCn = x.SentenceCn!,
                                            SentenceEn = x.SentenceEn!
                                        };
                                        toUpdate.Sentences.Add(sentTemp);
                                    });
                                }
                            }
                            else if (field.StartsWith("sentenceCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid sentenceId))
                                {
                                    toUpdate.Sentences.Where(x => x.Id == sentenceId).Select(x => x).Single().SentenceCn = wordDto.Sentences.Where(x => x.Id == sentenceId).Select(x => x.SentenceCn).Single()!;
                                }
                            }
                            else if (field.StartsWith("remMethod"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid remMethodId))
                                {
                                    toUpdate.RemMethods.Where(x => x.Id == remMethodId).Select(x => x).Single().Method = wordDto.RemMethods.Where(x => x.Id == remMethodId).Select(x => x.Method).Single()!;
                                }
                                else
                                {
                                    wordDto.RemMethods.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var remTemp = new RemMethod
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId,
                                            Method = x.Method!
                                        };
                                        toUpdate.RemMethods.Add(remTemp);
                                    });
                                }
                            }
                            else if (field.StartsWith("questionEn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().QuestionEn = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.QuestionEn).Single()!;
                                }
                                else
                                {
                                    wordDto.SingleChoiceQuestions.Where((x) => x.Id == null).Select(x => x).ToList().ForEach(x =>
                                    {
                                        var scqTemp = new SingleChoiceQuestion
                                        {
                                            Id = Guid.NewGuid(),
                                            WordId = toUpdate.Id,
                                            BookId = toUpdate.BookId,
                                            QuestionEn = x.QuestionEn,
                                            AnsExplainCn = x.AnsExplainCn,
                                            AnswerIndex = x.AnswerIndex,
                                            Choice1 = x.Choice1,
                                            Choice2 = x.Choice2,
                                            Choice3 = x.Choice3,
                                            Choice4 = x.Choice4,
                                        };
                                        toUpdate.SingleChoiceQuestions.Add(scqTemp);
                                    });
                                }
                            }
                            else if (field.StartsWith("choice1"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().Choice1 = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.Choice1).Single()!;
                                }

                            }
                            else if (field.StartsWith("choice2"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().Choice2 = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.Choice2).Single()!;
                                }
                            }
                            else if (field.StartsWith("choice3"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().Choice3 = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.Choice3).Single()!;
                                }
                            }
                            else if (field.StartsWith("choice4"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().Choice4 = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.Choice4).Single()!;
                                }
                            }
                            else if (field.StartsWith("ansExplainCn"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().AnsExplainCn = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.AnsExplainCn).Single()!;
                                }
                            }
                            else if (field.StartsWith("ans"))
                            {
                                if (Guid.TryParse(field.Split(':')[1], out Guid scqId))
                                {
                                    toUpdate.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x).Single().AnswerIndex = wordDto.SingleChoiceQuestions.Where(x => x.Id == scqId).Select(x => x.AnswerIndex).Single()!;
                                }
                            }
                        }
                        _context.Update(toUpdate);
                        if (await _context.SaveChangesAsync(cancellationToken) > 0)
                        {
                            var toUpdateDto = _mapper.Map<WordDto>(toUpdate);
                            return new ReturnValue
                            {
                                Value = toUpdateDto,
                                HttpCode = HttpStatusCode.OK,
                                Msg = $"{wordDto.WordName} in {wordDto.BookNameCh} has been updated!"
                            };
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Value = wordDto,
                                HttpCode = HttpStatusCode.BadRequest,
                                Msg = $"{wordDto.WordName} in {wordDto.BookNameCh} has NOT been updated!"
                            };
                        }

                    }
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId:{bookId}; wordDto:{wordNFieldsDto.WordDto}",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId:{bookId}; wordDto:{wordNFieldsDto.WordDto}",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ReturnValue> DelFieldofAWordAsync(Guid userId, Guid fieldId, string field, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    dynamic? toDel = null;
                    if (field == "tran")
                    {
                        toDel = await _context.Trans.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.Trans.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "phrase")
                    {
                        toDel = await _context.Phrases.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.Phrases.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "syno")
                    {
                        toDel = await _context.Synonymous.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.Synonymous.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "cognate")
                    {
                        toDel = await _context.Cognates.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.Cognates.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "sentence")
                    {
                        toDel = await _context.Sentences.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.Sentences.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "remMethod")
                    {
                        toDel = await _context.RemMethods.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.RemMethods.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    else if (field == "scq")
                    {
                        toDel = await _context.SingleChoiceQuestions.Where(x => x.Id == fieldId).SingleOrDefaultAsync(cancellationToken);
                        if (toDel != null)
                        {
                            _context.SingleChoiceQuestions.Remove(toDel);
                        }
                        else
                        {
                            return new ReturnValue
                            {
                                Msg = "未找到删除项",
                                HttpCode = HttpStatusCode.BadRequest,
                                Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                            };
                        }
                    }
                    if (await _context.SaveChangesAsync(cancellationToken) > 0)
                    {
                        return new ReturnValue
                        {
                            Msg = $"成功删除{field}",
                            HttpCode = HttpStatusCode.OK,
                            Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                        };
                    }
                    else
                    {
                        return new ReturnValue
                        {
                            Msg = $"删除{field}失败",
                            HttpCode = HttpStatusCode.BadRequest,
                            Value = $"userId:{userId}; fieldId:{fieldId}; field:{field}"
                        };
                    }
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; fieldId: {fieldId}; field: {field}",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            
        }

        public async Task<ReturnValue> LearnTransAsync( Guid userId,  Guid bookId,  int wordsCount, CancellationToken cancellationToken)
        {

            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    List<LearnTransDto> res = new();
                    int oldCount = (int)Math.Ceiling(wordsCount * 0.6);
                    int statusOneCount = (int)Math.Ceiling(oldCount * 0.2);
                    int statusTwoCount = (int)Math.Ceiling(oldCount * 0.2);
                    int statusThreeCount = oldCount - statusOneCount - statusTwoCount;

                    #region construct old word list
                    var statusNullWordIds = _context.Progresses.AsNoTracking().Where(x => x.UserId == userId && x.BookId == bookId && x.Status == 0).Select(x => x.WordId);
                    var statusOneWordIds = _context.Progresses.AsNoTracking().Where(x => x.UserId == userId && x.BookId == bookId && x.Status == 1).Take(statusOneCount).Select(x => x.WordId);
                    var statusTwoWordIds = _context.Progresses.AsNoTracking().Where(x => x.UserId == userId && x.BookId == bookId && x.Status == 2).Take(statusTwoCount).Select(x => x.WordId);
                    var statusThreeWordIds = _context.Progresses.AsNoTracking().Where(x => x.UserId == userId && x.BookId == bookId && x.Status == 3).Take(statusThreeCount).Select(x => x.WordId);
                    var oldWordId = ((statusOneWordIds ?? Enumerable.Empty<Guid>()).Union(statusTwoWordIds?? Enumerable.Empty<Guid>())).Union(statusThreeWordIds ?? Enumerable.Empty<Guid>());
                    var oldWordsDto = await _context.Words.AsNoTracking().Where(x => oldWordId.Contains(x.Id))
                        .Select(x => new WordDto
                        {
                            Id = x.Id,
                            WordName = x.WordName,
                            BookId = bookId,
                            BookNameCh = x.Book.BookNameCh,
                            Ukphone = x.Ukphone,
                            Usphone = x.Usphone,
                            Ukspeech = x.Ukspeech,
                            Usspeech = x.Usspeech,
                            Phone = x.Phone,
                            Speech = x.Speech,
                            Cognates = _mapper.Map<List<CognatesDto>>(x.Cognates),
                            Phrases = _mapper.Map<List<PhraseDto>>(x.Phrases),
                            RemMethods = _mapper.Map<List<RemMethodDto>>(x.RemMethods),
                            Sentences = _mapper.Map<List<SentenceDto>>(x.Sentences),
                            SingleChoiceQuestions = _mapper.Map<List<SingleChoiceQuestionDto>>(x.SingleChoiceQuestions),
                            Synonymous = _mapper.Map<List<SynonymousDto>>(x.Synonymous),
                            Trans = _mapper.Map<List<TranDto>>(x.Trans),
                            Progress = x.Progresses
                            .Where(p => p.UserId == userId && p.BookId == bookId && p.WordId == x.Id)
                            .Select(p => new ProgressDto
                            {
                                Id = p.Id,
                                UserId = p.UserId,
                                BookId = p.BookId,
                                WordId = p.WordId,
                                Status = p.Status,
                                LastUpdateTime = p.LastUpdateTime,
                            }).ToArray()
                        }).Take(oldWordId.Count()).ToListAsync();
                    #endregion

                    #region construct New Word List
                    int newCount = wordsCount - oldWordId.Count();
                    var newWordsDto = await _context.Words.AsNoTracking()
                        .Where(x => x.BookId == bookId && oldWordId.All(id => id != x.Id) && statusNullWordIds.All(id=>id!=x.Id))
                        .OrderBy(x => Guid.NewGuid())
                        .Take(newCount)
                        .Select(x => new WordDto
                        {
                            Id = x.Id,
                            WordName = x.WordName,
                            BookId = bookId,
                            BookNameCh = x.Book.BookNameCh,
                            Ukphone = x.Ukphone,
                            Usphone = x.Usphone,
                            Ukspeech = x.Ukspeech,
                            Usspeech = x.Usspeech,
                            Phone = x.Phone,
                            Speech = x.Speech,
                            Cognates = _mapper.Map<List<CognatesDto>>(x.Cognates),
                            Phrases = _mapper.Map<List<PhraseDto>>(x.Phrases),
                            RemMethods = _mapper.Map<List<RemMethodDto>>(x.RemMethods),
                            Sentences = _mapper.Map<List<SentenceDto>>(x.Sentences),
                            SingleChoiceQuestions = _mapper.Map<List<SingleChoiceQuestionDto>>(x.SingleChoiceQuestions),
                            Synonymous = _mapper.Map<List<SynonymousDto>>(x.Synonymous),
                            Trans = _mapper.Map<List<TranDto>>(x.Trans)
                        }).ToListAsync();
                    #endregion

                    var wordsDtoList = oldWordsDto.Concat(newWordsDto).ToList();
                    var trueOptions = wordsDtoList
                        .Select(x => x.Trans)
                        .Select(y => new MiniTrans
                        {
                            TransAllInOne = string.Join(";", y.Select(p => $"{p.Pos!.Trim()}: {p.TransCn}").ToList()),
                            WordId = (Guid)y.Select(p => p.WordId).Distinct().Single()!
                        });
                    var wrongOptions = await _context.Words
                        .Where(x => !wordsDtoList.Select(w => w.Id).ToList().Contains(x.Id))
                        .OrderBy(x => Guid.NewGuid())
                        .Take(3 * wordsCount)
                        .Select(x => x.Trans)
                        .Select(y => new MiniTrans
                        {
                            TransAllInOne = string.Join(";", y.Select(p => $"{p.Pos!.Trim()}: {p.TransCn}").ToList()),
                            WordId = (Guid)y.Select(p => p.WordId).Distinct().Single()!
                        }).ToListAsync();
                    var wrongOptionsQue = new Queue<MiniTrans>(wrongOptions.OrderBy(x => Guid.NewGuid().GetHashCode()));
                    foreach (var wordDto in wordsDtoList)
                    {
                        var rightIdx = new Random(Guid.NewGuid().GetHashCode()).Next(0, 4);
                        string trueOption = trueOptions.Where(x => x.WordId == wordDto.Id).Select(x => x.TransAllInOne).Single();
                        List<string> options = new List<string>();
                        options.Add(wrongOptionsQue.Dequeue().TransAllInOne);
                        options.Add(wrongOptionsQue.Dequeue().TransAllInOne);
                        options.Add(wrongOptionsQue.Dequeue().TransAllInOne);
                        options.Insert(rightIdx, trueOption);
                        LearnTransDto temp = new LearnTransDto
                        {
                            WordDto = wordDto,
                            Options = options,
                            RightAnIdx = rightIdx,
                            WordStatus = wordDto.Progress?.Where(x => x.BookId == bookId && x.UserId == userId).Select(x => x.Status).Single(),
                        };
                        res.Add(temp);
                    }
                    return new ReturnValue
                    {
                        Msg = $"Return {wordsCount} words for user:{userId} in book:{bookId} to learn words",
                        Value = res,
                        HttpCode = HttpStatusCode.OK,
                    };
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = userId,
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId: {bookId}; wordsCount:{wordsCount}",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            
        }

        public async Task<ReturnValue> UpdateUserSettings(Guid userId, UserSettings newSettings, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.Where(x=>x.Id == userId).SingleOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                return new ReturnValue
                {
                    Value = userId,
                    Msg = "User not found!",
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            user.Settings = JsonConvert.SerializeObject(newSettings);
            await _context.SaveChangesAsync(cancellationToken);
            return new ReturnValue
            {
                Value = _mapper.Map<UserDto>(user),
                Msg = $"User {user.Id} 's user settings have been updated!",
                HttpCode = HttpStatusCode.OK
            };
        }

        public async Task<ReturnValue> UpdateProgress(Guid userId, Guid bookId, List<LearnTransDto> words, CancellationToken cancellationToken)
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    UserBook userBook = _context.UserBooks.Include(x=>x.Book).Where(x=>x.UserId == userId && x.BookId == bookId).Single();
                    int wordsDoneOffset = 0;
                    // 发送来的数据，含有两部分，一部分是已经在Progress表中含有的单词，即重复背诵的单词，需要更新Progress表；
                    // 另一部分是新背的单词，在Progress表中不存在数据，根据wordStatus和wordStatusOffset决定需不需要新添加到Progress表中。
                    #region 先更新老单词的状态
                    var oldWordsIds = words.Where(x => x.WordStatus != null).Select(x => x.WordDto.Id).ToList();
                    var toUpdateProgressEntries = _context.Progresses.Where(x => x.UserId == userId && x.BookId == bookId && oldWordsIds.Contains(x.WordId)).ToList();
                    foreach (var toUpdateProgressEntry in toUpdateProgressEntries)
                    {
                        int? wordStatusOffset = words.Where(x => x.WordDto.Id == toUpdateProgressEntry.WordId).Select(x => x.WordStatusOffset).Single()!;
                        var newStatus = toUpdateProgressEntry.Status + wordStatusOffset;
                        if (newStatus == 4)
                        {
                            newStatus = 3;
                        }
                        else if(newStatus == 0)
                        {
                            wordsDoneOffset++;
                        }
                        toUpdateProgressEntry.Status = (int)newStatus;
                        toUpdateProgressEntry.LastUpdateTime = DateTime.Now;
                    }
                    #endregion

                    #region 再插入新单词的状态
                    var newWordsIds = words.Where(x => x.WordStatus == null && x.WordStatusOffset != null).Select(x => x.WordDto.Id).ToList();
                    List<Progress> toInsertProgressEntries = new List<Progress>();
                    foreach (var wordId in newWordsIds)
                    {
                        Progress newProgressEntry = new Progress
                        {
                            Id = Guid.NewGuid(),
                            BookId = bookId,
                            UserId = userId,
                            LastUpdateTime = DateTime.Now,
                            WordId = (Guid)wordId!,
                            Status = words.Where(x=>x.WordDto.Id == wordId).Select(x=>x.WordStatusOffset).Single() == -1 ? 0 : 1,
                        };
                        toInsertProgressEntries.Add(newProgressEntry);
                        if(newProgressEntry.Status == 0)
                        {
                            wordsDoneOffset++;
                        }
                    }
                    userBook.WordsDone = userBook.WordsDone + wordsDoneOffset;
                    userBook.LastUpdateTime = DateTime.Now;
                    await _context.Progresses.AddRangeAsync(toInsertProgressEntries);
                    await _context.SaveChangesAsync();

                    // 如果这本书的WordsDone和WordsCount相等，表示本书已背完一遍，这时从Progress表里删除本用户的本书的所有数据。并且将RepeatTimes++。
                    if (userBook.WordsDone >= userBook.Book.WordsCount)
                    {
                        var toDel = _context.Progresses.Where(x=>x.Status == 0 && x.UserId == userId && x.BookId == bookId).ToList();
                        _context.RemoveRange(toDel);
                        userBook.Finished = true;
                        userBook.RepeatTimes += 1;
                        _context.SaveChanges();
                    }
                    var userBooks = await _context.UserBooks.Include(x => x.Book).Where(x=>x.UserId == userId).ToListAsync();

                    #endregion
                    return new ReturnValue
                    {
                        Msg = $"userId: {userId}; bookId: {bookId}; " +
                        $"{toUpdateProgressEntries.Count} Progresses updated and {toInsertProgressEntries.Count} Progresses inserted!" +
                        $"{userBook.WordsDone} words Done!",
                        Value = userBooks,
                        HttpCode = HttpStatusCode.OK,
                    };
                }
                else {
                    cancellationToken.ThrowIfCancellationRequested();
                    return new ReturnValue
                    {
                        Value = $"userId: {userId}; bookId: {bookId};",
                        Msg = "用户取消操作",
                        HttpCode = HttpStatusCode.BadRequest
                    };
                }
            }
            catch (OperationCanceledException ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId: {bookId};",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = $"userId: {userId}; bookId: {bookId};",
                    Msg = ex.Message,
                    HttpCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}